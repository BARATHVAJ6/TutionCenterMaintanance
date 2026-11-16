using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Fees;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public FeesController(AppDbContext context)
    {
        _context = context;
    }

    // --- Fee Plans ---
    [HttpGet("plans")]
    public async Task<ActionResult<ApiResponse<List<FeePlanDto>>>> GetFeePlans(
        [FromQuery] int? gradeId, [FromQuery] int? academicYearId)
    {
        var query = _context.FeePlans
            .Include(f => f.Grade)
            .Include(f => f.AcademicYear)
            .AsQueryable();

        if (gradeId.HasValue) query = query.Where(f => f.GradeId == gradeId.Value);
        if (academicYearId.HasValue) query = query.Where(f => f.AcademicYearId == academicYearId.Value);

        var list = await query.Select(f => new FeePlanDto
        {
            Id = f.Id,
            Name = f.Name,
            Frequency = f.Frequency,
            Amount = f.Amount,
            GradeId = f.GradeId,
            GradeName = f.Grade.Name,
            AcademicYearId = f.AcademicYearId,
            AcademicYearName = f.AcademicYear.Name,
            IsActive = f.IsActive
        }).ToListAsync();

        return Ok(ApiResponse<List<FeePlanDto>>.Ok(list));
    }

    [HttpPost("plans")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<FeePlanDto>>> CreateFeePlan([FromBody] FeePlanDto dto)
    {
        var plan = new FeePlan
        {
            Name = dto.Name,
            Frequency = dto.Frequency,
            Amount = dto.Amount,
            GradeId = dto.GradeId,
            AcademicYearId = dto.AcademicYearId,
            IsActive = dto.IsActive
        };

        _context.FeePlans.Add(plan);
        await _context.SaveChangesAsync();
        dto.Id = plan.Id;
        return Ok(ApiResponse<FeePlanDto>.Ok(dto, "Fee plan created successfully"));
    }

    // --- Discounts ---
    [HttpGet("discounts")]
    public async Task<ActionResult<ApiResponse<List<DiscountDto>>>> GetDiscounts()
    {
        var discounts = await _context.Discounts
            .Select(d => new DiscountDto
            {
                Id = d.Id,
                Name = d.Name,
                Type = d.Type,
                Value = d.Value,
                IsPercentage = d.IsPercentage
            })
            .ToListAsync();

        return Ok(ApiResponse<List<DiscountDto>>.Ok(discounts));
    }

    [HttpPost("discounts")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<DiscountDto>>> CreateDiscount([FromBody] DiscountDto dto)
    {
        var discount = new Discount
        {
            Name = dto.Name,
            Type = dto.Type,
            Value = dto.Value,
            IsPercentage = dto.IsPercentage
        };

        _context.Discounts.Add(discount);
        await _context.SaveChangesAsync();
        dto.Id = discount.Id;
        return Ok(ApiResponse<DiscountDto>.Ok(dto, "Discount created successfully"));
    }

    // --- Assign Fee Plan ---
    [HttpPost("assign")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> AssignFeePlan([FromBody] AssignFeePlanRequest request)
    {
        var feePlan = await _context.FeePlans.FindAsync(request.FeePlanId);
        if (feePlan == null)
            return BadRequest(ApiResponse<bool>.Fail("Fee plan not found"));

        var student = await _context.Students.FindAsync(request.StudentId);
        if (student == null)
            return BadRequest(ApiResponse<bool>.Fail("Student not found"));

        var studentFeePlan = await _context.StudentFeePlans
            .Include(sfp => sfp.Installments)
            .FirstOrDefaultAsync(sfp => sfp.StudentId == request.StudentId && sfp.FeePlanId == request.FeePlanId);

        if (studentFeePlan != null)
        {
            studentFeePlan.DiscountId = request.DiscountId;
            studentFeePlan.FinalAmount = request.FinalAmount;
            studentFeePlan.ModifiedAt = DateTime.UtcNow;
        }
        else
        {
            studentFeePlan = new StudentFeePlan
            {
                StudentId = request.StudentId,
                FeePlanId = request.FeePlanId,
                DiscountId = request.DiscountId,
                FinalAmount = request.FinalAmount
            };
            _context.StudentFeePlans.Add(studentFeePlan);
        }

        await _context.SaveChangesAsync();

        if (request.GenerateInstallments)
        {
            // Remove unpaid installments if re-assigning
            var unpaidInstallments = studentFeePlan.Installments
                .Where(i => i.PaidAmount == 0)
                .ToList();
            if (unpaidInstallments.Any())
            {
                _context.FeeInstallments.RemoveRange(unpaidInstallments);
            }

            var startDate = request.StartDate ?? DateTime.Today;
            var installments = GenerateInstallmentSchedule(studentFeePlan.Id, feePlan.Frequency, request.FinalAmount, startDate);
            _context.FeeInstallments.AddRange(installments);
            await _context.SaveChangesAsync();
        }

        return Ok(ApiResponse<bool>.Ok(true, "Fee plan assigned with installment schedule"));
    }

    private static List<FeeInstallment> GenerateInstallmentSchedule(int studentFeePlanId, FeeFrequency frequency, decimal totalAmount, DateTime startDate)
    {
        var list = new List<FeeInstallment>();

        int count = frequency switch
        {
            FeeFrequency.OneTime => 1,
            FeeFrequency.Monthly => 10, // Standard 10-month academic session
            FeeFrequency.Quarterly => 4,
            FeeFrequency.HalfYearly => 2,
            _ => 1
        };

        decimal baseAmount = Math.Floor(totalAmount / count);
        decimal remainder = totalAmount - (baseAmount * count);

        for (int i = 0; i < count; i++)
        {
            decimal instAmount = baseAmount + (i == count - 1 ? remainder : 0);
            DateTime dueDate;
            string title;

            switch (frequency)
            {
                case FeeFrequency.OneTime:
                    dueDate = startDate;
                    title = "Full Course Fee";
                    break;
                case FeeFrequency.Monthly:
                    var targetMonth = startDate.AddMonths(i);
                    dueDate = new DateTime(targetMonth.Year, targetMonth.Month, Math.Min(10, DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month)));
                    title = $"{targetMonth:MMMM yyyy} Fee";
                    break;
                case FeeFrequency.Quarterly:
                    var quarterMonth = startDate.AddMonths(i * 3);
                    dueDate = new DateTime(quarterMonth.Year, quarterMonth.Month, Math.Min(10, DateTime.DaysInMonth(quarterMonth.Year, quarterMonth.Month)));
                    title = $"Quarter {i + 1} Fee ({quarterMonth:MMM yyyy})";
                    break;
                case FeeFrequency.HalfYearly:
                    var halfMonth = startDate.AddMonths(i * 6);
                    dueDate = new DateTime(halfMonth.Year, halfMonth.Month, Math.Min(10, DateTime.DaysInMonth(halfMonth.Year, halfMonth.Month)));
                    title = $"Term {i + 1} Fee ({halfMonth:MMM yyyy})";
                    break;
                default:
                    dueDate = startDate.AddMonths(i);
                    title = $"Installment {i + 1}";
                    break;
            }

            list.Add(new FeeInstallment
            {
                StudentFeePlanId = studentFeePlanId,
                Title = title,
                DueDate = dueDate,
                Amount = instAmount,
                PaidAmount = 0,
                Status = PaymentStatus.Pending
            });
        }

        return list;
    }

    // --- Payments ---
    [HttpPost("pay")]
    [Authorize(Policy = Permissions.PaymentsRecord)]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> RecordPayment([FromBody] RecordPaymentRequest request)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _context.Users.FindAsync(currentUserId);
        var student = await _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId);

        if (student == null)
            return BadRequest(ApiResponse<PaymentDto>.Fail("Student not found"));

        // Generate official Receipt Number: REC-YYYYMM-XXXX
        var currentYearMonth = DateTime.UtcNow.ToString("yyyyMM");
        var paymentSeq = await _context.Payments.CountAsync() + 1;
        var receiptNumber = $"REC-{currentYearMonth}-{paymentSeq:D4}";

        var payment = new Payment
        {
            ReceiptNumber = receiptNumber,
            StudentId = request.StudentId,
            FeeInstallmentId = request.FeeInstallmentId,
            Amount = request.Amount,
            PaymentDate = request.PaymentDate,
            PaymentMode = request.PaymentMode,
            TransactionRef = request.TransactionRef,
            Remarks = request.Remarks,
            ReceivedByUserId = currentUserId
        };

        _context.Payments.Add(payment);

        // Auto-allocate to installments
        if (request.FeeInstallmentId.HasValue)
        {
            var installment = await _context.FeeInstallments.FindAsync(request.FeeInstallmentId.Value);
            if (installment != null)
            {
                installment.PaidAmount += request.Amount;
                installment.Status = installment.PaidAmount >= installment.Amount
                    ? PaymentStatus.Paid
                    : PaymentStatus.PartiallyPaid;
                installment.ModifiedAt = DateTime.UtcNow;
            }
        }
        else if (request.AutoAllocateToInstallments)
        {
            var pendingInstallments = await _context.FeeInstallments
                .Include(fi => fi.StudentFeePlan)
                .Where(fi => fi.StudentFeePlan.StudentId == request.StudentId && fi.Status != PaymentStatus.Paid)
                .OrderBy(fi => fi.DueDate)
                .ToListAsync();

            decimal remainingToAllocate = request.Amount;
            foreach (var inst in pendingInstallments)
            {
                if (remainingToAllocate <= 0) break;
                decimal needed = inst.Amount - inst.PaidAmount;
                decimal alloc = Math.Min(remainingToAllocate, needed);
                inst.PaidAmount += alloc;
                remainingToAllocate -= alloc;
                inst.Status = inst.PaidAmount >= inst.Amount ? PaymentStatus.Paid : PaymentStatus.PartiallyPaid;
                inst.ModifiedAt = DateTime.UtcNow;

                if (payment.FeeInstallmentId == null)
                {
                    payment.FeeInstallmentId = inst.Id;
                }
            }
        }

        // Generate system in-app notification for fee payment
        _context.Notifications.Add(new Notification
        {
            Title = "Fee Payment Received",
            Message = $"Receipt {receiptNumber}: ₹{request.Amount:N0} received for {student.FirstName} {student.LastName} ({student.Grade.Name}) via {request.PaymentMode}.",
            Type = "Success",
            UserId = currentUserId,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        var installmentTitle = string.Empty;
        if (payment.FeeInstallmentId.HasValue)
        {
            var inst = await _context.FeeInstallments.FindAsync(payment.FeeInstallmentId.Value);
            installmentTitle = inst?.Title;
        }

        var dto = new PaymentDto
        {
            Id = payment.Id,
            ReceiptNumber = payment.ReceiptNumber,
            StudentId = payment.StudentId,
            StudentName = $"{student.FirstName} {student.LastName}".Trim(),
            GradeName = student.Grade.Name,
            BoardName = student.Board.Name,
            FeeInstallmentId = payment.FeeInstallmentId,
            InstallmentTitle = installmentTitle,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMode = payment.PaymentMode,
            TransactionRef = payment.TransactionRef,
            Remarks = payment.Remarks,
            ReceivedByName = currentUser?.FullName ?? "Staff"
        };

        return Ok(ApiResponse<PaymentDto>.Ok(dto, "Payment recorded successfully"));
    }

    [HttpGet("payments")]
    [Authorize(Policy = Permissions.PaymentsView)]
    public async Task<ActionResult<ApiResponse<List<PaymentDto>>>> GetPayments([FromQuery] int? studentId = null)
    {
        var query = _context.Payments
            .Include(p => p.Student).ThenInclude(s => s.Grade)
            .Include(p => p.Student).ThenInclude(s => s.Board)
            .Include(p => p.FeeInstallment)
            .Include(p => p.ReceivedByUser)
            .AsQueryable();

        if (studentId.HasValue)
        {
            query = query.Where(p => p.StudentId == studentId.Value);
        }

        var list = await query
            .OrderByDescending(p => p.PaymentDate)
            .ThenByDescending(p => p.Id)
            .Take(150)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                ReceiptNumber = p.ReceiptNumber,
                StudentId = p.StudentId,
                StudentName = $"{p.Student.FirstName} {p.Student.LastName}".Trim(),
                GradeName = p.Student.Grade.Name,
                BoardName = p.Student.Board.Name,
                FeeInstallmentId = p.FeeInstallmentId,
                InstallmentTitle = p.FeeInstallment != null ? p.FeeInstallment.Title : null,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMode = p.PaymentMode,
                TransactionRef = p.TransactionRef,
                Remarks = p.Remarks,
                ReceivedByName = p.ReceivedByUser != null ? p.ReceivedByUser.FullName : "Staff"
            })
            .ToListAsync();

        return Ok(ApiResponse<List<PaymentDto>>.Ok(list));
    }

    // --- Student Summaries & Overdue Defaulters ---
    [HttpGet("student-summaries")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<StudentFeeSummaryDto>>>> GetStudentFeeSummaries(
        [FromQuery] int? gradeId = null,
        [FromQuery] PaymentStatus? status = null)
    {
        var studentsQuery = _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .Include(s => s.StudentFeePlans).ThenInclude(sfp => sfp.FeePlan)
            .Include(s => s.StudentFeePlans).ThenInclude(sfp => sfp.Installments)
            .Include(s => s.Payments)
            .Where(s => s.IsActive);

        if (gradeId.HasValue)
        {
            studentsQuery = studentsQuery.Where(s => s.GradeId == gradeId.Value);
        }

        var students = await studentsQuery.ToListAsync();
        var summaries = new List<StudentFeeSummaryDto>();

        foreach (var s in students)
        {
            var totalFee = s.StudentFeePlans.Sum(sfp => sfp.FinalAmount);
            var paidAmount = s.Payments.Sum(p => p.Amount);
            var due = totalFee - paidAmount;

            var activePlan = s.StudentFeePlans.FirstOrDefault();
            var allInstallments = s.StudentFeePlans.SelectMany(sfp => sfp.Installments).ToList();

            var overdueInstallments = allInstallments
                .Where(i => i.Status != PaymentStatus.Paid && i.DueDate.Date < DateTime.Today)
                .ToList();

            var nextDue = allInstallments
                .Where(i => i.Status != PaymentStatus.Paid && i.DueDate.Date >= DateTime.Today)
                .OrderBy(i => i.DueDate)
                .Select(i => (DateTime?)i.DueDate)
                .FirstOrDefault();

            var payStatus = PaymentStatus.Paid;
            if (totalFee == 0)
            {
                payStatus = PaymentStatus.Paid;
            }
            else if (overdueInstallments.Any() || (due > 0 && DateTime.Today.Day > 10))
            {
                payStatus = PaymentStatus.Overdue;
            }
            else if (paidAmount == 0)
            {
                payStatus = PaymentStatus.Pending;
            }
            else if (due > 0)
            {
                payStatus = PaymentStatus.PartiallyPaid;
            }

            if (status.HasValue && payStatus != status.Value)
            {
                continue;
            }

            summaries.Add(new StudentFeeSummaryDto
            {
                StudentId = s.Id,
                StudentName = $"{s.FirstName} {s.LastName}".Trim(),
                GradeName = s.Grade.Name,
                BoardName = s.Board.Name,
                ParentPhone = s.Parent.FatherPhone,
                WhatsAppPhone = !string.IsNullOrEmpty(s.Parent.FatherWhatsApp) ? s.Parent.FatherWhatsApp : s.Parent.FatherPhone,
                FeePlanName = activePlan?.FeePlan?.Name ?? "No Plan",
                TotalFee = totalFee,
                PaidAmount = paidAmount,
                Status = payStatus,
                NextDueDate = nextDue,
                OverdueInstallmentsCount = overdueInstallments.Count,
                OverdueAmount = overdueInstallments.Sum(i => i.Amount - i.PaidAmount)
            });
        }

        return Ok(ApiResponse<List<StudentFeeSummaryDto>>.Ok(summaries));
    }

    // --- Student Fee Ledger ---
    [HttpGet("ledger/{studentId}")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<StudentFeeLedgerDto>>> GetStudentLedger(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .Include(s => s.StudentFeePlans).ThenInclude(sfp => sfp.FeePlan)
            .Include(s => s.StudentFeePlans).ThenInclude(sfp => sfp.Installments)
            .Include(s => s.Payments).ThenInclude(p => p.ReceivedByUser)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
            return NotFound(ApiResponse<StudentFeeLedgerDto>.Fail("Student not found"));

        var totalFee = student.StudentFeePlans.Sum(sfp => sfp.FinalAmount);
        var paidAmount = student.Payments.Sum(p => p.Amount);
        var due = totalFee - paidAmount;

        var allInstallments = student.StudentFeePlans
            .SelectMany(sfp => sfp.Installments)
            .OrderBy(i => i.DueDate)
            .Select(i => new FeeInstallmentDto
            {
                Id = i.Id,
                StudentFeePlanId = i.StudentFeePlanId,
                Title = i.Title,
                DueDate = i.DueDate,
                Amount = i.Amount,
                PaidAmount = i.PaidAmount,
                Status = i.Status
            })
            .ToList();

        var payments = student.Payments
            .OrderByDescending(p => p.PaymentDate)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                ReceiptNumber = p.ReceiptNumber,
                StudentId = p.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}".Trim(),
                GradeName = student.Grade.Name,
                BoardName = student.Board.Name,
                FeeInstallmentId = p.FeeInstallmentId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMode = p.PaymentMode,
                TransactionRef = p.TransactionRef,
                Remarks = p.Remarks,
                ReceivedByName = p.ReceivedByUser != null ? p.ReceivedByUser.FullName : "Staff"
            })
            .ToList();

        var payStatus = PaymentStatus.Paid;
        if (totalFee == 0) payStatus = PaymentStatus.Paid;
        else if (allInstallments.Any(i => i.IsOverdue)) payStatus = PaymentStatus.Overdue;
        else if (paidAmount == 0) payStatus = PaymentStatus.Pending;
        else if (due > 0) payStatus = PaymentStatus.PartiallyPaid;

        var activePlan = student.StudentFeePlans.FirstOrDefault()?.FeePlan?.Name ?? "General Plan";

        var ledger = new StudentFeeLedgerDto
        {
            StudentId = student.Id,
            StudentName = $"{student.FirstName} {student.LastName}".Trim(),
            GradeName = student.Grade.Name,
            BoardName = student.Board.Name,
            FatherName = student.Parent.FatherName,
            PrimaryPhone = student.Parent.FatherPhone,
            WhatsAppPhone = !string.IsNullOrEmpty(student.Parent.FatherWhatsApp) ? student.Parent.FatherWhatsApp : student.Parent.FatherPhone,
            FeePlanName = activePlan,
            TotalFee = totalFee,
            PaidAmount = paidAmount,
            Status = payStatus,
            Installments = allInstallments,
            Payments = payments
        };

        return Ok(ApiResponse<StudentFeeLedgerDto>.Ok(ledger));
    }

    // --- Official Payment Receipt ---
    [HttpGet("receipt/{paymentId}")]
    [Authorize(Policy = Permissions.PaymentsView)]
    public async Task<ActionResult<ApiResponse<PaymentReceiptDto>>> GetPaymentReceipt(int paymentId)
    {
        var payment = await _context.Payments
            .Include(p => p.Student).ThenInclude(s => s.Grade)
            .Include(p => p.Student).ThenInclude(s => s.Board)
            .Include(p => p.Student).ThenInclude(s => s.Parent)
            .Include(p => p.Student).ThenInclude(s => s.StudentFeePlans)
            .Include(p => p.ReceivedByUser)
            .FirstOrDefaultAsync(p => p.Id == paymentId);

        if (payment == null)
            return NotFound(ApiResponse<PaymentReceiptDto>.Fail("Payment record not found"));

        var centerSettings = await _context.CenterSettings.FirstOrDefaultAsync() ?? new CenterSettings();

        var student = payment.Student;
        var totalFee = student.StudentFeePlans.Sum(sfp => sfp.FinalAmount);
        var totalPaidSoFar = await _context.Payments
            .Where(p => p.StudentId == student.Id && p.PaymentDate <= payment.PaymentDate)
            .SumAsync(p => p.Amount);
        var remainingBalance = Math.Max(0, totalFee - totalPaidSoFar);

        var receipt = new PaymentReceiptDto
        {
            PaymentId = payment.Id,
            ReceiptNumber = !string.IsNullOrEmpty(payment.ReceiptNumber) ? payment.ReceiptNumber : $"REC-{payment.PaymentDate:yyyyMM}-{payment.Id:D4}",
            PaymentDate = payment.PaymentDate,
            StudentId = student.Id,
            StudentName = $"{student.FirstName} {student.LastName}".Trim(),
            GradeName = student.Grade.Name,
            BoardName = student.Board.Name,
            SchoolName = student.SchoolName,
            ParentName = student.Parent.FatherName,
            ParentPhone = student.Parent.FatherPhone,
            AmountPaid = payment.Amount,
            PaymentMode = payment.PaymentMode,
            TransactionRef = payment.TransactionRef,
            Remarks = payment.Remarks,
            ReceivedByName = payment.ReceivedByUser?.FullName ?? "Administrator",
            TotalFee = totalFee,
            TotalPaidSoFar = totalPaidSoFar,
            RemainingBalance = remainingBalance,
            CenterName = centerSettings.CenterName,
            CenterTagline = centerSettings.Tagline,
            CenterAddress = centerSettings.Address,
            CenterPhone = centerSettings.Phone,
            CenterEmail = centerSettings.Email,
            CurrencySymbol = "₹"
        };

        return Ok(ApiResponse<PaymentReceiptDto>.Ok(receipt));
    }

    // --- Aging Defaulters Analysis ---
    [HttpGet("defaulters/summary")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<FeeDefaultersSummaryDto>>> GetDefaultersSummary()
    {
        var centerSettings = await _context.CenterSettings.FirstOrDefaultAsync() ?? new CenterSettings();
        var centerName = centerSettings.CenterName;

        var students = await _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .Include(s => s.StudentFeePlans).ThenInclude(sfp => sfp.FeePlan)
            .Include(s => s.StudentFeePlans).ThenInclude(sfp => sfp.Installments)
            .Include(s => s.Payments)
            .Where(s => s.IsActive)
            .ToListAsync();

        var summary = new FeeDefaultersSummaryDto();
        var today = DateTime.Today;

        foreach (var s in students)
        {
            var totalFee = s.StudentFeePlans.Sum(sfp => sfp.FinalAmount);
            var paidAmount = s.Payments.Sum(p => p.Amount);
            var overallDue = totalFee - paidAmount;

            var overdueInstallments = s.StudentFeePlans
                .SelectMany(sfp => sfp.Installments)
                .Where(i => i.Status != PaymentStatus.Paid && i.DueDate.Date < today)
                .OrderBy(i => i.DueDate)
                .ToList();

            if (!overdueInstallments.Any() && overallDue <= 0)
                continue;

            decimal overdueAmount = overdueInstallments.Any()
                ? overdueInstallments.Sum(i => i.Amount - i.PaidAmount)
                : overallDue;

            if (overdueAmount <= 0)
                continue;

            DateTime oldestDueDate = overdueInstallments.Any()
                ? overdueInstallments.First().DueDate
                : today.AddDays(-15);

            int daysOverdue = (int)(today - oldestDueDate.Date).TotalDays;
            if (daysOverdue < 1) daysOverdue = 1;

            string bracket;
            if (daysOverdue <= 15)
            {
                bracket = "1-15 days";
                summary.Count1To15Days++;
                summary.Amount1To15Days += overdueAmount;
            }
            else if (daysOverdue <= 30)
            {
                bracket = "16-30 days";
                summary.Count16To30Days++;
                summary.Amount16To30Days += overdueAmount;
            }
            else if (daysOverdue <= 60)
            {
                bracket = "31-60 days";
                summary.Count31To60Days++;
                summary.Amount31To60Days += overdueAmount;
            }
            else
            {
                bracket = "60+ days";
                summary.Count60PlusDays++;
                summary.Amount60PlusDays += overdueAmount;
            }

            var studentName = $"{s.FirstName} {s.LastName}".Trim();
            var parentName = !string.IsNullOrEmpty(s.Parent.FatherName) ? s.Parent.FatherName : "Parent";
            var phone = !string.IsNullOrEmpty(s.Parent.FatherWhatsApp) ? s.Parent.FatherWhatsApp : s.Parent.FatherPhone;

            var reminderMessage = $"Dear {parentName},\n\nThis is a gentle reminder from {centerName} regarding the tuition fee payment for {studentName} ({s.Grade.Name}).\nPending Overdue Amount: ₹{overdueAmount:N0}\nDue Date: {oldestDueDate:dd-MMM-yyyy} ({daysOverdue} days overdue).\n\nPlease arrange for the payment at your earliest convenience.\n\nThank you,\n{centerName}";

            summary.Defaulters.Add(new FeeDefaulterItemDto
            {
                StudentId = s.Id,
                StudentName = studentName,
                GradeName = s.Grade.Name,
                BoardName = s.Board.Name,
                ParentName = parentName,
                ParentPhone = s.Parent.FatherPhone,
                WhatsAppPhone = phone,
                FeePlanName = s.StudentFeePlans.FirstOrDefault()?.FeePlan?.Name ?? "General Plan",
                TotalFee = totalFee,
                PaidAmount = paidAmount,
                OverdueAmount = overdueAmount,
                OldestDueDate = oldestDueDate,
                MaxDaysOverdue = daysOverdue,
                AgingBracket = bracket,
                FormattedReminderMessage = reminderMessage
            });

            summary.TotalOverdueAmount += overdueAmount;
            summary.TotalDefaulterCount++;
        }

        summary.Defaulters = summary.Defaulters.OrderByDescending(d => d.MaxDaysOverdue).ToList();

        return Ok(ApiResponse<FeeDefaultersSummaryDto>.Ok(summary));
    }

    // --- Financial Analytics: Monthly Trend ---
    [HttpGet("analytics/monthly-trend")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<MonthlyCollectionDto>>>> GetMonthlyCollectionTrend()
    {
        var twelveMonthsAgo = DateTime.UtcNow.AddMonths(-11);
        var startDate = new DateTime(twelveMonthsAgo.Year, twelveMonthsAgo.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var payments = await _context.Payments
            .Where(p => p.PaymentDate >= startDate)
            .ToListAsync();

        var result = new List<MonthlyCollectionDto>();

        for (int i = 0; i < 12; i++)
        {
            var target = startDate.AddMonths(i);
            var monthPayments = payments.Where(p => p.PaymentDate.Year == target.Year && p.PaymentDate.Month == target.Month).ToList();

            result.Add(new MonthlyCollectionDto
            {
                MonthName = target.ToString("MMM yyyy"),
                Year = target.Year,
                Month = target.Month,
                CollectedAmount = monthPayments.Sum(p => p.Amount),
                TransactionCount = monthPayments.Count
            });
        }

        return Ok(ApiResponse<List<MonthlyCollectionDto>>.Ok(result));
    }

    // --- Financial Analytics: Payment Modes ---
    [HttpGet("analytics/payment-modes")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<PaymentModeBreakdownDto>>>> GetPaymentModeBreakdown()
    {
        var payments = await _context.Payments.ToListAsync();
        var totalAmount = payments.Sum(p => p.Amount);

        var modes = Enum.GetValues<PaymentMode>();
        var result = new List<PaymentModeBreakdownDto>();

        foreach (var mode in modes)
        {
            var modePayments = payments.Where(p => p.PaymentMode == mode).ToList();
            var modeTotal = modePayments.Sum(p => p.Amount);

            result.Add(new PaymentModeBreakdownDto
            {
                Mode = mode,
                TotalAmount = modeTotal,
                TransactionCount = modePayments.Count,
                Percentage = totalAmount > 0 ? Math.Round((modeTotal / totalAmount) * 100, 1) : 0
            });
        }

        return Ok(ApiResponse<List<PaymentModeBreakdownDto>>.Ok(result.OrderByDescending(r => r.TotalAmount).ToList()));
    }

    // --- Financial Analytics: Grade Wise Summary ---
    [HttpGet("analytics/grades")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<GradeFinancialSummaryDto>>>> GetGradeFinancialSummaries()
    {
        var grades = await _context.Grades
            .Include(g => g.Students).ThenInclude(s => s.StudentFeePlans)
            .Include(g => g.Students).ThenInclude(s => s.Payments)
            .OrderBy(g => g.Id)
            .ToListAsync();

        var result = new List<GradeFinancialSummaryDto>();

        foreach (var g in grades)
        {
            var activeStudents = g.Students.Where(s => s.IsActive).ToList();
            var totalExpected = activeStudents.SelectMany(s => s.StudentFeePlans).Sum(sfp => sfp.FinalAmount);
            var totalCollected = activeStudents.SelectMany(s => s.Payments).Sum(p => p.Amount);

            int defaulterCount = 0;
            foreach (var s in activeStudents)
            {
                var sTotal = s.StudentFeePlans.Sum(sfp => sfp.FinalAmount);
                var sPaid = s.Payments.Sum(p => p.Amount);
                if (sTotal > sPaid)
                {
                    defaulterCount++;
                }
            }

            result.Add(new GradeFinancialSummaryDto
            {
                GradeId = g.Id,
                GradeName = g.Name,
                StudentCount = activeStudents.Count,
                TotalExpected = totalExpected,
                TotalCollected = totalCollected,
                DefaulterCount = defaulterCount
            });
        }

        return Ok(ApiResponse<List<GradeFinancialSummaryDto>>.Ok(result));
    }
}
