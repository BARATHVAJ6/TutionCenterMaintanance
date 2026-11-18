using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Services.Interfaces;
using TutionCenterMaintanance.Shared.DTOs.Fees;
using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Services;

public class FeesService : IFeesService
{
    private readonly AppDbContext _context;

    public FeesService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentMonthlyStatusDto>> GetMonthlyStatusAsync(int year, int month, int? gradeId = null, CancellationToken ct = default)
    {
        var startOfMonth = new DateTime(year, month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

        var studentsQuery = _context.Students
            .AsNoTracking()
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .Where(s => s.IsActive && !s.IsDeleted);

        if (gradeId.HasValue && gradeId.Value > 0)
        {
            studentsQuery = studentsQuery.Where(s => s.GradeId == gradeId.Value);
        }

        var students = await studentsQuery
            .OrderBy(s => s.Grade != null ? s.Grade.OrderIndex : 0)
            .ThenBy(s => s.FirstName)
            .ToListAsync(ct);

        var payments = await _context.Payments
            .AsNoTracking()
            .Where(p => !p.IsDeleted && p.PaymentDate >= startOfMonth && p.PaymentDate <= endOfMonth)
            .ToListAsync(ct);

        var paymentsByStudent = payments
            .GroupBy(p => p.StudentId)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    TotalPaid = g.Sum(p => p.Amount),
                    Count = g.Count(),
                    LastPaymentDate = g.Max(p => (DateTime?)p.PaymentDate)
                }
            );

        var result = new List<StudentMonthlyStatusDto>();

        foreach (var student in students)
        {
            var fee = student.MonthlyFeeAmount;
            paymentsByStudent.TryGetValue(student.Id, out var payInfo);
            var paid = payInfo?.TotalPaid ?? 0m;

            PaymentStatus status;
            if (fee <= 0)
            {
                status = PaymentStatus.Pending;
            }
            else if (paid >= fee)
            {
                status = PaymentStatus.Paid;
            }
            else if (paid > 0)
            {
                status = PaymentStatus.PartiallyPaid;
            }
            else
            {
                status = PaymentStatus.Pending;
            }

            var phone = student.Parent != null
                ? (!string.IsNullOrWhiteSpace(student.Parent.FatherPhone) ? student.Parent.FatherPhone : student.Parent.MotherPhone ?? "")
                : "";

            var parentName = student.Parent != null
                ? (!string.IsNullOrWhiteSpace(student.Parent.FatherName) ? student.Parent.FatherName : student.Parent.MotherName ?? "")
                : "";

            result.Add(new StudentMonthlyStatusDto
            {
                StudentId = student.Id,
                StudentName = $"{student.FirstName} {student.LastName}".Trim(),
                GradeName = student.Grade?.Name ?? "—",
                BoardName = student.Board?.Name ?? "—",
                ParentName = parentName,
                PrimaryPhone = phone,
                PhotoUrl = student.PhotoUrl,
                MonthlyFeeAmount = fee,
                PaidAmount = paid,
                Status = status,
                LastPaymentDate = payInfo?.LastPaymentDate,
                PaymentCount = payInfo?.Count ?? 0,
                Month = month,
                Year = year
            });
        }

        return result;
    }

    public async Task<bool> SetMonthlyFeeAsync(SetMonthlyFeeRequest request, CancellationToken ct = default)
    {
        var student = await _context.Students.FindAsync(new object[] { request.StudentId }, ct);
        if (student == null) return false;

        student.MonthlyFeeAmount = Math.Max(0, request.MonthlyFeeAmount);
        student.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<PaymentDto> RecordPaymentAsync(RecordPaymentRequest request, string? currentUserId, CancellationToken ct = default)
    {
        var student = await _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, ct);

        if (student == null)
            throw new KeyNotFoundException("Student not found.");

        var count = await _context.Payments.CountAsync(ct) + 1;
        var receiptNo = $"RCT-{DateTime.Today:yyyyMM}-{count:D4}";

        var payment = new Payment
        {
            ReceiptNumber = receiptNo,
            StudentId = request.StudentId,
            Amount = request.Amount,
            PaymentDate = request.PaymentDate.Date != default ? request.PaymentDate : DateTime.Today,
            PaymentMode = request.PaymentMode,
            TransactionRef = request.TransactionRef?.Trim(),
            Remarks = request.Remarks?.Trim(),
            ReceivedByUserId = currentUserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(ct);

        return new PaymentDto
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
            ReceivedByName = "Staff"
        };
    }

    public async Task<List<PaymentDto>> GetPaymentsAsync(int? studentId = null, CancellationToken ct = default)
    {
        var query = _context.Payments
            .AsNoTracking()
            .Include(p => p.Student).ThenInclude(s => s.Grade)
            .Include(p => p.Student).ThenInclude(s => s.Board)
            .Include(p => p.ReceivedByUser)
            .AsQueryable();

        if (studentId.HasValue)
        {
            query = query.Where(p => p.StudentId == studentId.Value);
        }

        return await query
            .OrderByDescending(p => p.PaymentDate)
            .ThenByDescending(p => p.Id)
            .Take(100)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                ReceiptNumber = p.ReceiptNumber,
                StudentId = p.StudentId,
                StudentName = p.Student != null ? $"{p.Student.FirstName} {p.Student.LastName}".Trim() : string.Empty,
                GradeName = p.Student != null && p.Student.Grade != null ? p.Student.Grade.Name : string.Empty,
                BoardName = p.Student != null && p.Student.Board != null ? p.Student.Board.Name : string.Empty,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMode = p.PaymentMode,
                TransactionRef = p.TransactionRef,
                Remarks = p.Remarks,
                ReceivedByName = p.ReceivedByUser != null ? p.ReceivedByUser.FullName : "Admin"
            })
            .ToListAsync(ct);
    }

    public async Task<PaymentReceiptDto?> GetPaymentReceiptAsync(int paymentId, CancellationToken ct = default)
    {
        var payment = await _context.Payments
            .AsNoTracking()
            .Include(p => p.Student).ThenInclude(s => s.Grade)
            .Include(p => p.Student).ThenInclude(s => s.Board)
            .Include(p => p.Student).ThenInclude(s => s.Parent)
            .Include(p => p.ReceivedByUser)
            .FirstOrDefaultAsync(p => p.Id == paymentId, ct);

        if (payment == null || payment.Student == null)
            return null;

        var center = await _context.CenterSettings.AsNoTracking().FirstOrDefaultAsync(ct) ?? new CenterSettings();

        var payMonth = payment.PaymentDate.Month;
        var payYear = payment.PaymentDate.Year;
        var startMonth = new DateTime(payYear, payMonth, 1);
        var endMonth = startMonth.AddMonths(1).AddTicks(-1);

        var monthPayments = await _context.Payments
            .AsNoTracking()
            .Where(p => p.StudentId == payment.StudentId && p.PaymentDate >= startMonth && p.PaymentDate <= endMonth)
            .SumAsync(p => p.Amount, ct);

        var monthlyFee = payment.Student.MonthlyFeeAmount;
        var remaining = Math.Max(0, monthlyFee - monthPayments);

        return new PaymentReceiptDto
        {
            PaymentId = payment.Id,
            ReceiptNumber = payment.ReceiptNumber,
            PaymentDate = payment.PaymentDate,
            StudentName = $"{payment.Student.FirstName} {payment.Student.LastName}".Trim(),
            GradeName = payment.Student.Grade?.Name ?? "",
            BoardName = payment.Student.Board?.Name ?? "",
            SchoolName = payment.Student.SchoolName,
            ParentName = payment.Student.Parent?.FatherName ?? payment.Student.Parent?.MotherName ?? "",
            ParentPhone = payment.Student.Parent?.FatherPhone ?? payment.Student.Parent?.MotherPhone ?? "",
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
            CurrencySymbol = center.CurrencySymbol ?? "₹"
        };
    }

    public async Task<StudentFeeLedgerDto?> GetStudentFeeLedgerAsync(int studentId, CancellationToken ct = default)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .FirstOrDefaultAsync(s => s.Id == studentId, ct);

        if (student == null)
            return null;

        var payments = await _context.Payments
            .AsNoTracking()
            .Where(p => p.StudentId == studentId)
            .OrderByDescending(p => p.PaymentDate)
            .ThenByDescending(p => p.Id)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                ReceiptNumber = p.ReceiptNumber,
                StudentId = p.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}".Trim(),
                GradeName = student.Grade != null ? student.Grade.Name : "",
                BoardName = student.Board != null ? student.Board.Name : "",
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMode = p.PaymentMode,
                TransactionRef = p.TransactionRef,
                Remarks = p.Remarks,
                ReceivedByName = "Staff"
            })
            .ToListAsync(ct);

        var totalPaid = payments.Sum(p => p.Amount);
        var totalFee = student.MonthlyFeeAmount;

        return new StudentFeeLedgerDto
        {
            StudentId = student.Id,
            StudentName = $"{student.FirstName} {student.LastName}".Trim(),
            GradeName = student.Grade?.Name ?? "",
            BoardName = student.Board?.Name ?? "",
            FatherName = student.Parent?.FatherName ?? "",
            PrimaryPhone = student.Parent?.FatherPhone ?? student.Parent?.MotherPhone ?? "",
            WhatsAppPhone = student.Parent?.FatherWhatsApp ?? student.Parent?.MotherWhatsApp,
            FeePlanName = "Monthly Tuition",
            TotalFee = totalFee,
            PaidAmount = totalPaid,
            Status = totalPaid >= totalFee && totalFee > 0 ? PaymentStatus.Paid : (totalPaid > 0 ? PaymentStatus.PartiallyPaid : PaymentStatus.Pending),
            Payments = payments
        };
    }
}
