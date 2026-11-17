using System.ComponentModel.DataAnnotations;
using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Shared.DTOs.Expenses;

public class ExpenseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ExpenseCategory Category { get; set; }
    public string CategoryName => Category.ToString();
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? PaidTo { get; set; }
    public string? Notes { get; set; }
    public string? RecordedByName { get; set; }
}

public class CreateExpenseRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public ExpenseCategory Category { get; set; } = ExpenseCategory.Rent;

    [Range(0.01, 10000000, ErrorMessage = "Expense amount must be greater than zero.")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.Today;

    [StringLength(100)]
    public string? PaidTo { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}
