using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
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
    private readonly AppDbContext _context;

    public ExpensesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<ExpenseDto>>>> GetExpenses(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        [FromQuery] ExpenseCategory? category = null)
    {
        var query = _context.Expenses
            .Include(e => e.RecordedByUser)
            .AsQueryable();

        if (month.HasValue && year.HasValue)
        {
            var start = new DateTime(year.Value, month.Value, 1);
            var end = start.AddMonths(1).AddTicks(-1);
            query = query.Where(e => e.Date >= start && e.Date <= end);
        }
        else if (year.HasValue)
        {
            var start = new DateTime(year.Value, 1, 1);
            var end = new DateTime(year.Value, 12, 31, 23, 59, 59);
            query = query.Where(e => e.Date >= start && e.Date <= end);
        }

        if (category.HasValue)
        {
            query = query.Where(e => e.Category == category.Value);
        }

        var expenses = await query
            .OrderByDescending(e => e.Date)
            .ThenByDescending(e => e.Id)
            .Select(e => new ExpenseDto
            {
                Id = e.Id,
                Title = e.Title,
                Category = e.Category,
                Amount = e.Amount,
                Date = e.Date,
                PaidTo = e.PaidTo,
                Notes = e.Notes,
                RecordedByName = e.RecordedByUser != null ? e.RecordedByUser.FullName : "Admin"
            })
            .ToListAsync();

        return Ok(ApiResponse<List<ExpenseDto>>.Ok(expenses));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<ExpenseDto>>> CreateExpense([FromBody] CreateExpenseRequest request)
    {
        if (request.Amount <= 0)
            return BadRequest(ApiResponse<ExpenseDto>.Fail("Amount must be greater than zero."));

        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(ApiResponse<ExpenseDto>.Fail("Expense title is required."));

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var expense = new Expense
        {
            Title = request.Title.Trim(),
            Category = request.Category,
            Amount = request.Amount,
            Date = request.Date.Date != default ? request.Date : DateTime.Today,
            PaidTo = request.PaidTo?.Trim(),
            Notes = request.Notes?.Trim(),
            RecordedByUserId = currentUserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            Id = expense.Id,
            Title = expense.Title,
            Category = expense.Category,
            Amount = expense.Amount,
            Date = expense.Date,
            PaidTo = expense.PaidTo,
            Notes = expense.Notes,
            RecordedByName = User.Identity?.Name ?? "Admin"
        };

        return Ok(ApiResponse<ExpenseDto>.Ok(dto, "Expense recorded successfully"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteExpense(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null)
            return NotFound(ApiResponse<bool>.Fail("Expense not found"));

        expense.IsDeleted = true;
        expense.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Expense deleted successfully"));
    }
}
