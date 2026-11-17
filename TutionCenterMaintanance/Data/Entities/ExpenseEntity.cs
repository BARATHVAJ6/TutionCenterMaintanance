using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Data.Entities;

public class Expense : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public ExpenseCategory Category { get; set; } = ExpenseCategory.Rent;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? PaidTo { get; set; }
    public string? Notes { get; set; }

    public string? RecordedByUserId { get; set; }
    public AppUser? RecordedByUser { get; set; }
}
