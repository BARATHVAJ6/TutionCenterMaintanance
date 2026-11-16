using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Data.Entities;

public class FeePlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public FeeFrequency Frequency { get; set; } = FeeFrequency.Monthly;
    public decimal Amount { get; set; }

    public int GradeId { get; set; }
    public Grade Grade { get; set; } = null!;

    public int AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public ICollection<StudentFeePlan> StudentFeePlans { get; set; } = new List<StudentFeePlan>();
}

public class Discount : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DiscountType Type { get; set; } = DiscountType.Scholarship;
    public decimal Value { get; set; }
    public bool IsPercentage { get; set; }

    public ICollection<StudentFeePlan> StudentFeePlans { get; set; } = new List<StudentFeePlan>();
}

public class StudentFeePlan : BaseEntity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int FeePlanId { get; set; }
    public FeePlan FeePlan { get; set; } = null!;

    public int? DiscountId { get; set; }
    public Discount? Discount { get; set; }

    public decimal FinalAmount { get; set; }

    public ICollection<FeeInstallment> Installments { get; set; } = new List<FeeInstallment>();
}

public class FeeInstallment : BaseEntity
{
    public int StudentFeePlanId { get; set; }
    public StudentFeePlan StudentFeePlan { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; } = 0;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public class Payment : BaseEntity
{
    public string ReceiptNumber { get; set; } = string.Empty;

    public int? FeeInstallmentId { get; set; }
    public FeeInstallment? FeeInstallment { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentMode PaymentMode { get; set; } = PaymentMode.Cash;
    public string? TransactionRef { get; set; }
    public string? Remarks { get; set; }

    public string? ReceivedByUserId { get; set; }
    public AppUser? ReceivedByUser { get; set; }
}
