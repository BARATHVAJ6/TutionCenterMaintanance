using TutionCenterMaintanance.Shared.DTOs.Expenses;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Services.Interfaces;

public interface IExpensesService
{
    Task<PaginatedResult<ExpenseDto>> GetExpensesAsync(
        int? month = null,
        int? year = null,
        ExpenseCategory? category = null,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default);

    Task<ExpenseDto> CreateExpenseAsync(CreateExpenseRequest request, string? currentUserId, CancellationToken ct = default);

    Task<bool> DeleteExpenseAsync(int id, CancellationToken ct = default);
}
