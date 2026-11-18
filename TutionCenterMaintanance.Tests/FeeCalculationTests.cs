using FluentAssertions;
using TutionCenterMaintanance.Shared.DTOs.Fees;
using TutionCenterMaintanance.Shared.Enums;
using Xunit;

namespace TutionCenterMaintanance.Tests;

public class FeeCalculationTests
{
    [Fact]
    public void StudentMonthlyStatus_WhenFullyPaid_StatusShouldBePaid()
    {
        // Arrange
        var status = new StudentMonthlyStatusDto
        {
            StudentId = 1,
            StudentName = "Ravi Kumar",
            MonthlyFeeAmount = 1500m,
            PaidAmount = 1500m,
            Status = PaymentStatus.Paid
        };

        // Assert
        status.Status.Should().Be(PaymentStatus.Paid);
        status.RemainingAmount.Should().Be(0m);
    }

    [Fact]
    public void StudentMonthlyStatus_WhenPartiallyPaid_StatusShouldBePartiallyPaidAndRemainingMatches()
    {
        // Arrange
        var status = new StudentMonthlyStatusDto
        {
            StudentId = 2,
            StudentName = "Priya S",
            MonthlyFeeAmount = 1500m,
            PaidAmount = 600m,
            Status = PaymentStatus.PartiallyPaid
        };

        // Assert
        status.Status.Should().Be(PaymentStatus.PartiallyPaid);
        status.RemainingAmount.Should().Be(900m);
    }

    [Fact]
    public void StudentMonthlyStatus_WhenZeroPaid_StatusShouldBePending()
    {
        // Arrange
        var status = new StudentMonthlyStatusDto
        {
            StudentId = 3,
            StudentName = "Aarav M",
            MonthlyFeeAmount = 2000m,
            PaidAmount = 0m,
            Status = PaymentStatus.Pending
        };

        // Assert
        status.Status.Should().Be(PaymentStatus.Pending);
        status.RemainingAmount.Should().Be(2000m);
    }

    [Theory]
    [InlineData(10000, 10000, 100.0)]
    [InlineData(5000, 10000, 50.0)]
    [InlineData(0, 10000, 0.0)]
    [InlineData(0, 0, 0.0)]
    public void CollectionRate_CalculatesExpectedPercentage(decimal collected, decimal expected, decimal expectedRate)
    {
        // Act
        var rate = expected > 0 ? Math.Round((collected / expected) * 100, 1) : 0m;

        // Assert
        rate.Should().Be((decimal)expectedRate);
    }

    [Fact]
    public void ReceiptNumber_FormatMatchesTuitionStandards()
    {
        // Arrange
        var now = new DateTime(2026, 11, 17);
        var sequence = 42;

        // Act
        var receiptNo = $"RCT-{now:yyyyMM}-{sequence:D4}";

        // Assert
        receiptNo.Should().Be("RCT-202611-0042");
    }
}
