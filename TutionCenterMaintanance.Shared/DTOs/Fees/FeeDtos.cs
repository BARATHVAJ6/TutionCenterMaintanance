using System.ComponentModel.DataAnnotations;
using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Shared.DTOs.Fees;

public class FeePlanDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public FeeFrequency Frequency { get; set; } = FeeFrequency.Monthly;
    public decimal Amount { get; set; }
    public int GradeId { get; set; }
    public string GradeName { get; set; } = string.Empty;
    public int AcademicYearId { get; set; }
    public string AcademicYearName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class DiscountDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public DiscountType Type { get; set; } = DiscountType.Scholarship;
    public decimal Value { get; set; }
    public bool IsPercentage { get; set; }
}

public class AssignFeePlanRequest
{
    public int StudentId { get; set; }
    public int FeePlanId { get; set; }
    public int? DiscountId { get; set; }
    public decimal FinalAmount { get; set; }
    public bool GenerateInstallments { get; set; } = true;
    public DateTime? StartDate { get; set; }
}

public class FeeInstallmentDto
{
    public int Id { get; set; }
    public int StudentFeePlanId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount => Amount - PaidAmount;
    public PaymentStatus Status { get; set; }
    public bool IsOverdue => Status != PaymentStatus.Paid && DueDate.Date < DateTime.Today;
    public int DaysOverdue => IsOverdue ? (int)(DateTime.Today - DueDate.Date).TotalDays : 0;
}

public class StudentFeeSummaryDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BoardName { get; set; } = string.Empty;
    public string ParentPhone { get; set; } = string.Empty;
    public string? WhatsAppPhone { get; set; }
    public string FeePlanName { get; set; } = string.Empty;
    public decimal TotalFee { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount => TotalFee - PaidAmount;
    public PaymentStatus Status { get; set; }
    public DateTime? NextDueDate { get; set; }
    public int OverdueInstallmentsCount { get; set; }
    public decimal OverdueAmount { get; set; }
}

public class StudentFeeLedgerDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BoardName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string PrimaryPhone { get; set; } = string.Empty;
    public string? WhatsAppPhone { get; set; }
    public string FeePlanName { get; set; } = string.Empty;
    public decimal TotalFee { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount => TotalFee - PaidAmount;
    public PaymentStatus Status { get; set; }
    public List<FeeInstallmentDto> Installments { get; set; } = new();
    public List<PaymentDto> Payments { get; set; } = new();
}

public class RecordPaymentRequest
{
    public int StudentId { get; set; }
    public int? FeeInstallmentId { get; set; }
    [Range(1, 1000000)]
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Today;
    public PaymentMode PaymentMode { get; set; } = PaymentMode.Cash;
    public string? TransactionRef { get; set; }
    public string? Remarks { get; set; }
    public bool AutoAllocateToInstallments { get; set; } = true;
}

public class PaymentDto
{
    public int Id { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BoardName { get; set; } = string.Empty;
    public int? FeeInstallmentId { get; set; }
    public string? InstallmentTitle { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMode PaymentMode { get; set; }
    public string? TransactionRef { get; set; }
    public string? Remarks { get; set; }
    public string ReceivedByName { get; set; } = string.Empty;
}

public class PaymentReceiptDto
{
    public int PaymentId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BoardName { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    public string ParentName { get; set; } = string.Empty;
    public string ParentPhone { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
    public PaymentMode PaymentMode { get; set; }
    public string? TransactionRef { get; set; }
    public string? Remarks { get; set; }
    public string ReceivedByName { get; set; } = string.Empty;
    public decimal TotalFee { get; set; }
    public decimal TotalPaidSoFar { get; set; }
    public decimal RemainingBalance { get; set; }
    public string CenterName { get; set; } = string.Empty;
    public string? CenterTagline { get; set; }
    public string? CenterAddress { get; set; }
    public string? CenterPhone { get; set; }
    public string? CenterEmail { get; set; }
    public string CurrencySymbol { get; set; } = "₹";
}

public class FeeDefaulterItemDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BoardName { get; set; } = string.Empty;
    public string ParentName { get; set; } = string.Empty;
    public string ParentPhone { get; set; } = string.Empty;
    public string? WhatsAppPhone { get; set; }
    public string FeePlanName { get; set; } = string.Empty;
    public decimal TotalFee { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal OverdueAmount { get; set; }
    public DateTime OldestDueDate { get; set; }
    public int MaxDaysOverdue { get; set; }
    public string AgingBracket { get; set; } = string.Empty; // "1-15 days", "16-30 days", "31-60 days", "60+ days"
    public string FormattedReminderMessage { get; set; } = string.Empty;
}

public class FeeDefaultersSummaryDto
{
    public decimal TotalOverdueAmount { get; set; }
    public int TotalDefaulterCount { get; set; }
    public int Count1To15Days { get; set; }
    public decimal Amount1To15Days { get; set; }
    public int Count16To30Days { get; set; }
    public decimal Amount16To30Days { get; set; }
    public int Count31To60Days { get; set; }
    public decimal Amount31To60Days { get; set; }
    public int Count60PlusDays { get; set; }
    public decimal Amount60PlusDays { get; set; }
    public List<FeeDefaulterItemDto> Defaulters { get; set; } = new();
}

public class MonthlyCollectionDto
{
    public string MonthName { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal CollectedAmount { get; set; }
    public int TransactionCount { get; set; }
}

public class PaymentModeBreakdownDto
{
    public PaymentMode Mode { get; set; }
    public string ModeName => Mode.ToString();
    public decimal TotalAmount { get; set; }
    public int TransactionCount { get; set; }
    public decimal Percentage { get; set; }
}

public class GradeFinancialSummaryDto
{
    public int GradeId { get; set; }
    public string GradeName { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public decimal TotalExpected { get; set; }
    public decimal TotalCollected { get; set; }
    public decimal TotalPending => TotalExpected - TotalCollected;
    public int DefaulterCount { get; set; }
    public decimal CollectionRate => TotalExpected > 0 ? Math.Round((TotalCollected / TotalExpected) * 100, 1) : 0;
}

public class StudentMonthlyStatusDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BoardName { get; set; } = string.Empty;
    public string ParentName { get; set; } = string.Empty;
    public string PrimaryPhone { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public decimal MonthlyFeeAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount => Math.Max(0, MonthlyFeeAmount - PaidAmount);
    public PaymentStatus Status { get; set; } // Paid, Pending (Unpaid), PartiallyPaid
    public DateTime? LastPaymentDate { get; set; }
    public int PaymentCount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class SetMonthlyFeeRequest
{
    public int StudentId { get; set; }
    [Range(0, 1000000)]
    public decimal MonthlyFeeAmount { get; set; }
}
