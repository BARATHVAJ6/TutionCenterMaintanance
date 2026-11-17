using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Shared.DTOs.Financials;

public class FinancialOverviewDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public string PeriodLabel { get; set; } = string.Empty;

    // --- Tuition Fees Side ---
    public decimal FeeRevenue { get; set; }
    public int ActiveStudentsCount { get; set; }
    public int PaidStudentsCount { get; set; }
    public int PartialStudentsCount { get; set; }
    public int UnpaidStudentsCount { get; set; }
    public decimal TotalExpectedFees { get; set; }
    public decimal CollectionRate => TotalExpectedFees > 0 ? Math.Round((FeeRevenue / TotalExpectedFees) * 100, 1) : 0;

    // --- Operating Expenses (Rent, EB, Salaries, Maintenance, etc. - NOT stationery) ---
    public decimal OperatingExpenses { get; set; }
    public List<ExpenseCategoryBreakdownDto> ExpenseBreakdown { get; set; } = new();

    // --- Stationery Auxiliary Business (Tracked Separately) ---
    public decimal StationerySalesRevenue { get; set; }
    public decimal StationeryPurchasesCost { get; set; }
    public decimal StationeryNetProfit => StationerySalesRevenue - StationeryPurchasesCost;
    public int StationeryItemsSoldCount { get; set; }

    // --- Net Operating Center Surplus (Tuition Fees - Operating Expenses) ---
    public decimal NetOperatingSurplus => FeeRevenue - OperatingExpenses;

    // --- Grand Totals (Fees + Stationery Overall) ---
    public decimal TotalCombinedRevenue => FeeRevenue + StationerySalesRevenue;
    public decimal TotalCombinedOutflow => OperatingExpenses + StationeryPurchasesCost;
    public decimal OverallNetProfit => TotalCombinedRevenue - TotalCombinedOutflow;

    // --- Comparison with Previous Month ---
    public decimal PrevMonthFeeRevenue { get; set; }
    public decimal PrevMonthOperatingExpenses { get; set; }
    public decimal PrevMonthStationeryRevenue { get; set; }
    public decimal PrevMonthNetProfit { get; set; }
    public decimal NetProfitChange => OverallNetProfit - PrevMonthNetProfit;
    public decimal RevenueChange => TotalCombinedRevenue - (PrevMonthFeeRevenue + PrevMonthStationeryRevenue);
}

public class ExpenseCategoryBreakdownDto
{
    public ExpenseCategory Category { get; set; }
    public string CategoryName => Category.ToString();
    public decimal TotalAmount { get; set; }
    public int TransactionCount { get; set; }
    public decimal Percentage { get; set; }
}

public class MonthlyFinancialTrendDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthLabel { get; set; } = string.Empty;
    public decimal FeeRevenue { get; set; }
    public decimal StationeryRevenue { get; set; }
    public decimal TotalRevenue => FeeRevenue + StationeryRevenue;
    public decimal OperatingExpenses { get; set; }
    public decimal StationeryPurchasesCost { get; set; }
    public decimal TotalExpenses => OperatingExpenses + StationeryPurchasesCost;
    public decimal NetProfit => TotalRevenue - TotalExpenses;
}
