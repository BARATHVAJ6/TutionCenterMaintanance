using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Services.Interfaces;
using TutionCenterMaintanance.Shared.DTOs.Expenses;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Services;

public class ExpensesService : IExpensesService
{
    private readonly AppDbContext _context;

    public ExpensesService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<ExpenseDto>> GetExpensesAsync(
        int? month = null,
        int? year = null,
        ExpenseCategory? category = null,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default)
    {
        pageSize = Math.Clamp(pageSize, 1, 100);
        page = Math.Max(1, page);

        var query = _context.Expenses
            .AsNoTracking()
            .Include(e => e.RecordedByUser)
            .Where(e => !e.IsDeleted)
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

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(e => e.Date)
            .ThenByDescending(e => e.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
            .ToListAsync(ct);

        return new PaginatedResult<ExpenseDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseRequest request, string? currentUserId, CancellationToken ct = default)
    {
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
        await _context.SaveChangesAsync(ct);

        return new ExpenseDto
        {
            Id = expense.Id,
            Title = expense.Title,
            Category = expense.Category,
            Amount = expense.Amount,
            Date = expense.Date,
            PaidTo = expense.PaidTo,
            Notes = expense.Notes,
            RecordedByName = "Staff"
        };
    }

    public async Task<bool> DeleteExpenseAsync(int id, CancellationToken ct = default)
    {
        var expense = await _context.Expenses.FindAsync(new object[] { id }, ct);
        if (expense == null) return false;

        expense.IsDeleted = true;
        expense.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return true;
    }
}
