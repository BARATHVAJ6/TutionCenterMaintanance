using FluentAssertions;
using TutionCenterMaintanance.Shared.DTOs.Financials;
using Xunit;

namespace TutionCenterMaintanance.Tests;

public class ExpenseTrackingTests
{
    [Fact]
    public void FinancialOverview_CalculatesNetProfitCorrectly()
    {
        // Arrange
        var tuitionRevenue = 45000m;
        var stationerySales = 3500m;
        var operatingExpenses = 12000m;
        var stationeryPurchases = 2000m;

        // Act
        var overview = new FinancialOverviewDto
        {
            FeeRevenue = tuitionRevenue,
            StationerySalesRevenue = stationerySales,
            OperatingExpenses = operatingExpenses,
            StationeryPurchasesCost = stationeryPurchases
        };

        // Assert
        overview.TotalCombinedRevenue.Should().Be(48500m);
        overview.TotalCombinedOutflow.Should().Be(14000m);
        overview.OverallNetProfit.Should().Be(34500m);
        overview.StationeryNetProfit.Should().Be(1500m);
        overview.NetOperatingSurplus.Should().Be(33000m);
    }

    [Fact]
    public void FinancialOverview_WhenExpensesExceedRevenue_OverallNetProfitIsNegative()
    {
        // Arrange
        var overview = new FinancialOverviewDto
        {
            FeeRevenue = 5000m,
            StationerySalesRevenue = 500m,
            OperatingExpenses = 10000m,
            StationeryPurchasesCost = 1000m
        };

        // Assert
        overview.TotalCombinedRevenue.Should().Be(5500m);
        overview.TotalCombinedOutflow.Should().Be(11000m);
        overview.OverallNetProfit.Should().Be(-5500m);
    }
}
