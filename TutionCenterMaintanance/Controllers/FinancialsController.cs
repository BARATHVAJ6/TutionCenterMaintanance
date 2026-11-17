using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Financials;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FinancialsController : ControllerBase
{
    private readonly AppDbContext _context;

    public FinancialsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("overview")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<FinancialOverviewDto>>> GetOverview(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null)
    {
        var targetMonth = month ?? DateTime.Today.Month;
        var targetYear = year ?? DateTime.Today.Year;

        var startOfMonth = new DateTime(targetYear, targetMonth, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

        // --- 1. Tuition Fees ---
        var feePaymentsThisMonth = await _context.Payments
            .Where(p => p.PaymentDate >= startOfMonth && p.PaymentDate <= endOfMonth)
            .ToListAsync();

        var feeRevenue = feePaymentsThisMonth.Sum(p => p.Amount);

        var activeStudents = await _context.Students
            .Include(s => s.Payments.Where(p => p.PaymentDate >= startOfMonth && p.PaymentDate <= endOfMonth))
            .Where(s => s.IsActive)
            .ToListAsync();

        var totalExpectedFees = activeStudents.Sum(s => s.MonthlyFeeAmount);

        int paidCount = 0;
        int partialCount = 0;
        int unpaidCount = 0;

        foreach (var s in activeStudents)
        {
            var paid = s.Payments.Sum(p => p.Amount);
            if (s.MonthlyFeeAmount <= 0)
            {
                if (paid > 0) paidCount++;
                else unpaidCount++;
            }
            else if (paid >= s.MonthlyFeeAmount)
            {
                paidCount++;
            }
            else if (paid > 0)
            {
                partialCount++;
            }
            else
            {
                unpaidCount++;
            }
        }

        // --- 2. Operating Expenses (Excluding Stationery Purchases) ---
        var expensesThisMonth = await _context.Expenses
            .Where(e => e.Date >= startOfMonth && e.Date <= endOfMonth)
            .ToListAsync();

        var operatingExpenses = expensesThisMonth.Sum(e => e.Amount);

        var expenseBreakdown = expensesThisMonth
            .GroupBy(e => e.Category)
            .Select(g => new ExpenseCategoryBreakdownDto
            {
                Category = g.Key,
                TotalAmount = g.Sum(e => e.Amount),
                TransactionCount = g.Count(),
                Percentage = operatingExpenses > 0 ? Math.Round((g.Sum(e => e.Amount) / operatingExpenses) * 100, 1) : 0
            })
            .OrderByDescending(b => b.TotalAmount)
            .ToList();

        // --- 3. Stationery Auxiliary Business ---
        var stationerySales = await _context.StationerySales
            .Where(s => s.Date >= startOfMonth && s.Date <= endOfMonth)
            .ToListAsync();

        var stationeryPurchases = await _context.StationeryPurchases
            .Where(p => p.Date >= startOfMonth && p.Date <= endOfMonth)
            .ToListAsync();

        var stationerySalesRevenue = stationerySales.Sum(s => s.TotalAmount);
        var stationeryPurchasesCost = stationeryPurchases.Sum(p => p.TotalCost);
        var stationeryItemsSold = stationerySales.Sum(s => s.Quantity);

        // --- 4. Previous Month for Comparison ---
        var prevMonthDate = startOfMonth.AddMonths(-1);
        var prevStart = new DateTime(prevMonthDate.Year, prevMonthDate.Month, 1);
        var prevEnd = prevStart.AddMonths(1).AddTicks(-1);

        var prevFeeRev = await _context.Payments
            .Where(p => p.PaymentDate >= prevStart && p.PaymentDate <= prevEnd)
            .SumAsync(p => (decimal?)p.Amount) ?? 0;

        var prevExpenses = await _context.Expenses
            .Where(e => e.Date >= prevStart && e.Date <= prevEnd)
            .SumAsync(e => (decimal?)e.Amount) ?? 0;

        var prevStationeryRev = await _context.StationerySales
            .Where(s => s.Date >= prevStart && s.Date <= prevEnd)
            .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;

        var prevStationeryCost = await _context.StationeryPurchases
            .Where(p => p.Date >= prevStart && p.Date <= prevEnd)
            .SumAsync(p => (decimal?)p.TotalCost) ?? 0;

        var prevNetProfit = (prevFeeRev + prevStationeryRev) - (prevExpenses + prevStationeryCost);

        var overview = new FinancialOverviewDto
        {
            Month = targetMonth,
            Year = targetYear,
            PeriodLabel = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(targetMonth)} {targetYear}",
            FeeRevenue = feeRevenue,
            ActiveStudentsCount = activeStudents.Count,
            PaidStudentsCount = paidCount,
            PartialStudentsCount = partialCount,
            UnpaidStudentsCount = unpaidCount,
            TotalExpectedFees = totalExpectedFees,
            OperatingExpenses = operatingExpenses,
            ExpenseBreakdown = expenseBreakdown,
            StationerySalesRevenue = stationerySalesRevenue,
            StationeryPurchasesCost = stationeryPurchasesCost,
            StationeryItemsSoldCount = stationeryItemsSold,
            PrevMonthFeeRevenue = prevFeeRev,
            PrevMonthOperatingExpenses = prevExpenses,
            PrevMonthStationeryRevenue = prevStationeryRev,
            PrevMonthNetProfit = prevNetProfit
        };

        return Ok(ApiResponse<FinancialOverviewDto>.Ok(overview));
    }

    [HttpGet("trends")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<MonthlyFinancialTrendDto>>>> GetTrends([FromQuery] int? year = null)
    {
        var targetYear = year ?? DateTime.Today.Year;
        var startOfYear = new DateTime(targetYear, 1, 1);
        var endOfYear = new DateTime(targetYear, 12, 31, 23, 59, 59);

        var feePayments = await _context.Payments
            .Where(p => p.PaymentDate >= startOfYear && p.PaymentDate <= endOfYear)
            .ToListAsync();

        var expenses = await _context.Expenses
            .Where(e => e.Date >= startOfYear && e.Date <= endOfYear)
            .ToListAsync();

        var stationerySales = await _context.StationerySales
            .Where(s => s.Date >= startOfYear && s.Date <= endOfYear)
            .ToListAsync();

        var stationeryPurchases = await _context.StationeryPurchases
            .Where(p => p.Date >= startOfYear && p.Date <= endOfYear)
            .ToListAsync();

        var trends = new List<MonthlyFinancialTrendDto>();

        for (int m = 1; m <= 12; m++)
        {
            var fees = feePayments.Where(p => p.PaymentDate.Month == m).Sum(p => p.Amount);
            var statRev = stationerySales.Where(s => s.Date.Month == m).Sum(s => s.TotalAmount);
            var opsExp = expenses.Where(e => e.Date.Month == m).Sum(e => e.Amount);
            var statCost = stationeryPurchases.Where(p => p.Date.Month == m).Sum(p => p.TotalCost);

            trends.Add(new MonthlyFinancialTrendDto
            {
                Year = targetYear,
                Month = m,
                MonthLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m),
                FeeRevenue = fees,
                StationeryRevenue = statRev,
                OperatingExpenses = opsExp,
                StationeryPurchasesCost = statCost
            });
        }

        return Ok(ApiResponse<List<MonthlyFinancialTrendDto>>.Ok(trends));
    }
}
