using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutionCenterMaintanance.Services.Interfaces;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Expenses;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IExpensesService _expensesService;

    public ExpensesController(IExpensesService expensesService)
    {
        _expensesService = expensesService;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ExpenseDto>>>> GetExpenses(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        [FromQuery] ExpenseCategory? category = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        CancellationToken ct = default)
    {
        var result = await _expensesService.GetExpensesAsync(month, year, category, page, pageSize, ct);
        return Ok(ApiResponse<PaginatedResult<ExpenseDto>>.Ok(result));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<ExpenseDto>>> CreateExpense(
        [FromBody] CreateExpenseRequest request,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<ExpenseDto>.Fail("Invalid expense data."));

        if (request.Amount <= 0)
            return BadRequest(ApiResponse<ExpenseDto>.Fail("Amount must be greater than zero."));

        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(ApiResponse<ExpenseDto>.Fail("Expense title is required."));

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var expense = await _expensesService.CreateExpenseAsync(request, currentUserId, ct);

        return CreatedAtAction(nameof(GetExpenses), new { id = expense.Id }, ApiResponse<ExpenseDto>.Ok(expense, "Expense recorded successfully"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteExpense(int id, CancellationToken ct = default)
    {
        var deleted = await _expensesService.DeleteExpenseAsync(id, ct);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Expense not found"));

        return Ok(ApiResponse<bool>.Ok(true, "Expense deleted successfully"));
    }
}
