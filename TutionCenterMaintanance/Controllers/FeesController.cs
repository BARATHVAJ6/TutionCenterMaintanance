using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    // --- Monthly Student Status (Paid / Partial / Unpaid) ---
    [HttpGet("monthly-status")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<StudentMonthlyStatusDto>>>> GetMonthlyStatus(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        [FromQuery] int? gradeId = null)
    {
        var targetMonth = month ?? DateTime.Today.Month;
        var targetYear = year ?? DateTime.Today.Year;

        var startOfMonth = new DateTime(targetYear, targetMonth, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

        var query = _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .Include(s => s.Payments.Where(p => p.PaymentDate >= startOfMonth && p.PaymentDate <= endOfMonth))
            .Where(s => s.IsActive);

        if (gradeId.HasValue && gradeId.Value > 0)
        {
            query = query.Where(s => s.GradeId == gradeId.Value);
        }

        var students = await query
            .OrderBy(s => s.Grade.Name)
            .ThenBy(s => s.FirstName)
            .ToListAsync();

        var list = students.Select(s =>
        {
            var paidAmount = s.Payments.Sum(p => p.Amount);
            var feeAmount = s.MonthlyFeeAmount;
            PaymentStatus status;

            if (feeAmount <= 0)
            {
                status = paidAmount > 0 ? PaymentStatus.Paid : PaymentStatus.Pending;
            }
            else if (paidAmount >= feeAmount)
            {
                status = PaymentStatus.Paid;
            }
            else if (paidAmount > 0)
            {
                status = PaymentStatus.PartiallyPaid;
            }
            else
            {
                status = PaymentStatus.Pending;
            }

            var lastPayment = s.Payments.OrderByDescending(p => p.PaymentDate).FirstOrDefault();

            return new StudentMonthlyStatusDto
            {
                StudentId = s.Id,
                StudentName = $"{s.FirstName} {s.LastName}".Trim(),
                GradeName = s.Grade.Name,
                BoardName = s.Board.Name,
                ParentName = !string.IsNullOrWhiteSpace(s.Parent?.FatherName) ? s.Parent.FatherName : (s.Parent?.MotherName ?? ""),
                PrimaryPhone = !string.IsNullOrWhiteSpace(s.Parent?.FatherPhone) ? s.Parent.FatherPhone : (s.Parent?.MotherPhone ?? ""),
                PhotoUrl = s.PhotoUrl,
                MonthlyFeeAmount = feeAmount,
                PaidAmount = paidAmount,
                Status = status,
                LastPaymentDate = lastPayment?.PaymentDate,
                PaymentCount = s.Payments.Count,
                Month = targetMonth,
                Year = targetYear
            };
        }).ToList();

        return Ok(ApiResponse<List<StudentMonthlyStatusDto>>.Ok(list));
    }

    // --- Set or Update Monthly Fee for Student ---
    [HttpPut("monthly-fee")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> SetMonthlyFee([FromBody] SetMonthlyFeeRequest request)
    {
        var student = await _context.Students.FindAsync(request.StudentId);
        if (student == null)
            return NotFound(ApiResponse<bool>.Fail("Student not found"));

        student.MonthlyFeeAmount = request.MonthlyFeeAmount;
        student.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Monthly fee updated successfully"));
    }

    // --- Record Cash Payment ---
    [HttpPost("pay")]
    [Authorize(Policy = Permissions.PaymentsRecord)]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> RecordPayment([FromBody] RecordPaymentRequest request)
    {
        if (request.Amount <= 0)
            return BadRequest(ApiResponse<PaymentDto>.Fail("Payment amount must be greater than zero"));

        var student = await _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId);

        if (student == null)
            return BadRequest(ApiResponse<PaymentDto>.Fail("Student not found"));

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentYearMonth = DateTime.UtcNow.ToString("yyyyMM");
        var count = await _context.Payments.CountAsync();
        var receiptNumber = $"REC-{currentYearMonth}-{(count + 1):D4}";

        var payment = new Payment
        {
            ReceiptNumber = receiptNumber,
            StudentId = request.StudentId,
            Amount = request.Amount,
            PaymentDate = request.PaymentDate.Date != default ? request.PaymentDate : DateTime.Today,
            PaymentMode = PaymentMode.Cash, // Cash transaction
            TransactionRef = request.TransactionRef,
            Remarks = request.Remarks,
            ReceivedByUserId = currentUserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        var paymentDto = new PaymentDto
        {
            Id = payment.Id,
            ReceiptNumber = payment.ReceiptNumber,
            StudentId = payment.StudentId,
            StudentName = $"{student.FirstName} {student.LastName}".Trim(),
            GradeName = student.Grade?.Name ?? "",
            BoardName = student.Board?.Name ?? "",
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMode = payment.PaymentMode,
            TransactionRef = payment.TransactionRef,
            Remarks = payment.Remarks,
            ReceivedByName = User.Identity?.Name ?? "Admin"
        };

        return Ok(ApiResponse<PaymentDto>.Ok(paymentDto, "Payment recorded successfully"));
    }

    // --- List Payments ---
    [HttpGet("payments")]
    [Authorize(Policy = Permissions.PaymentsView)]
    public async Task<ActionResult<ApiResponse<List<PaymentDto>>>> GetPayments([FromQuery] int? studentId = null)
    {
        var query = _context.Payments
            .Include(p => p.Student).ThenInclude(s => s.Grade)
            .Include(p => p.Student).ThenInclude(s => s.Board)
            .Include(p => p.ReceivedByUser)
            .AsQueryable();

        if (studentId.HasValue)
        {
            query = query.Where(p => p.StudentId == studentId.Value);
        }

        var payments = await query
            .OrderByDescending(p => p.PaymentDate)
            .ThenByDescending(p => p.Id)
            .Take(100)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                ReceiptNumber = p.ReceiptNumber,
                StudentId = p.StudentId,
                StudentName = $"{p.Student.FirstName} {p.Student.LastName}".Trim(),
                GradeName = p.Student.Grade.Name,
                BoardName = p.Student.Board.Name,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMode = p.PaymentMode,
                TransactionRef = p.TransactionRef,
                Remarks = p.Remarks,
                ReceivedByName = p.ReceivedByUser != null ? p.ReceivedByUser.FullName : "Admin"
            })
            .ToListAsync();

        return Ok(ApiResponse<List<PaymentDto>>.Ok(payments));
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
            .Include(p => p.ReceivedByUser)
            .FirstOrDefaultAsync(p => p.Id == paymentId);

        if (payment == null)
            return NotFound(ApiResponse<PaymentReceiptDto>.Fail("Payment record not found"));

        var center = await _context.CenterSettings.FirstOrDefaultAsync() ?? new CenterSettings();

        // Calculate student's total paid in the month of this payment
        var payMonth = payment.PaymentDate.Month;
        var payYear = payment.PaymentDate.Year;
        var startMonth = new DateTime(payYear, payMonth, 1);
        var endMonth = startMonth.AddMonths(1).AddTicks(-1);

        var monthPayments = await _context.Payments
            .Where(p => p.StudentId == payment.StudentId && p.PaymentDate >= startMonth && p.PaymentDate <= endMonth)
            .SumAsync(p => p.Amount);

        var monthlyFee = payment.Student.MonthlyFeeAmount;
        var remaining = Math.Max(0, monthlyFee - monthPayments);

        var dto = new PaymentReceiptDto
        {
            PaymentId = payment.Id,
            ReceiptNumber = payment.ReceiptNumber,
            PaymentDate = payment.PaymentDate,
            StudentId = payment.StudentId,
            StudentName = $"{payment.Student.FirstName} {payment.Student.LastName}".Trim(),
            GradeName = payment.Student.Grade.Name,
            BoardName = payment.Student.Board.Name,
            SchoolName = payment.Student.SchoolName,
            ParentName = !string.IsNullOrWhiteSpace(payment.Student.Parent?.FatherName)
                ? payment.Student.Parent.FatherName
                : (payment.Student.Parent?.MotherName ?? ""),
            ParentPhone = !string.IsNullOrWhiteSpace(payment.Student.Parent?.FatherPhone)
                ? payment.Student.Parent.FatherPhone
                : (payment.Student.Parent?.MotherPhone ?? ""),
            AmountPaid = payment.Amount,
            PaymentMode = payment.PaymentMode,
            TransactionRef = payment.TransactionRef,
            Remarks = payment.Remarks,
            ReceivedByName = payment.ReceivedByUser?.FullName ?? "Admin",
            TotalFee = monthlyFee,
            TotalPaidSoFar = monthPayments,
            RemainingBalance = remaining,
            CenterName = center.CenterName,
            CenterTagline = center.Tagline,
            CenterAddress = center.Address,
            CenterPhone = center.Phone,
            CenterEmail = center.Email,
            CurrencySymbol = center.CurrencySymbol
        };

        return Ok(ApiResponse<PaymentReceiptDto>.Ok(dto));
    }

    // --- Student Fee Ledger (History) ---
    [HttpGet("ledger/{studentId}")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<StudentFeeLedgerDto>>> GetStudentFeeLedger(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
            return NotFound(ApiResponse<StudentFeeLedgerDto>.Fail("Student not found"));

        var payments = await _context.Payments
            .Where(p => p.StudentId == studentId)
            .OrderByDescending(p => p.PaymentDate)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                ReceiptNumber = p.ReceiptNumber,
                StudentId = p.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}".Trim(),
                GradeName = student.Grade.Name,
                BoardName = student.Board.Name,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMode = p.PaymentMode,
                TransactionRef = p.TransactionRef,
                Remarks = p.Remarks,
                ReceivedByName = "Admin"
            })
            .ToListAsync();

        var totalPaid = payments.Sum(p => p.Amount);

        var ledger = new StudentFeeLedgerDto
        {
            StudentId = student.Id,
            StudentName = $"{student.FirstName} {student.LastName}".Trim(),
            GradeName = student.Grade.Name,
            BoardName = student.Board.Name,
            FatherName = !string.IsNullOrWhiteSpace(student.Parent?.FatherName)
                ? student.Parent.FatherName
                : (student.Parent?.MotherName ?? ""),
            PrimaryPhone = !string.IsNullOrWhiteSpace(student.Parent?.FatherPhone)
                ? student.Parent.FatherPhone
                : (student.Parent?.MotherPhone ?? ""),
            FeePlanName = $"₹{student.MonthlyFeeAmount:N0} / month",
            TotalFee = student.MonthlyFeeAmount,
            PaidAmount = totalPaid,
            Status = student.MonthlyFeeAmount > 0 && totalPaid >= student.MonthlyFeeAmount ? PaymentStatus.Paid : PaymentStatus.Pending,
            Payments = payments
        };

        return Ok(ApiResponse<StudentFeeLedgerDto>.Ok(ledger));
    }
}
