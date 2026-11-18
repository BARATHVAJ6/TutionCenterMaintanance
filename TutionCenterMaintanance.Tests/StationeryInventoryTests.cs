using FluentAssertions;
using TutionCenterMaintanance.Shared.DTOs.Stationery;
using Xunit;

namespace TutionCenterMaintanance.Tests;

public class StationeryInventoryTests
{
    [Fact]
    public void StationeryItem_StockCalculation_ReflectsPurchasesMinusSales()
    {
        // Arrange
        var item = new StationeryItemDto
        {
            Id = 1,
            Name = "A4 200pg Notebook",
            Unit = "Each",
            DefaultSellingPrice = 35m,
            TotalPurchasedQuantity = 50,
            TotalSoldQuantity = 18,
            TotalPurchaseCost = 1100m, // ₹22 each
            TotalSalesRevenue = 630m   // ₹35 each
        };

        // Act
        var calculatedStock = item.TotalPurchasedQuantity - item.TotalSoldQuantity;

        // Assert
        calculatedStock.Should().Be(32);
        item.ProfitMargin.Should().Be(234m); // 630 - (22 * 18 = 396) = 234
    }

    [Fact]
    public void StationeryItem_ZeroSales_ProfitMarginIsZero()
    {
        // Arrange
        var item = new StationeryItemDto
        {
            Id = 2,
            Name = "Geometry Box",
            Unit = "Box",
            DefaultSellingPrice = 120m,
            TotalPurchasedQuantity = 10,
            TotalSoldQuantity = 0,
            TotalPurchaseCost = 800m,
            TotalSalesRevenue = 0m
        };

        // Assert
        item.ProfitMargin.Should().Be(0m);
    }

    [Theory]
    [InlineData(10, 5, true)]
    [InlineData(10, 10, true)]
    [InlineData(10, 11, false)]
    [InlineData(0, 1, false)]
    public void StockValidation_PreventsSellingMoreThanAvailable(int availableStock, int requestedQuantity, bool isAllowed)
    {
        // Act
        var canFulfill = requestedQuantity <= availableStock;

        // Assert
        canFulfill.Should().Be(isAllowed);
    }
}
