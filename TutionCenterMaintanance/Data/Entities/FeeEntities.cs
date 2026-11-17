using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Data.Entities;

public class Payment : BaseEntity
{
    public string ReceiptNumber { get; set; } = string.Empty;

    public int? FeeInstallmentId { get; set; }

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
