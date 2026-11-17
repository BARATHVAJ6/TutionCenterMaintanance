using System.ComponentModel.DataAnnotations;

namespace TutionCenterMaintanance.Shared.DTOs.Stationery;

public class StationeryItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Unit { get; set; } = "Each";
    public decimal DefaultSellingPrice { get; set; }
    public int CurrentStock { get; set; }
    public int TotalPurchasedQuantity { get; set; }
    public int TotalSoldQuantity { get; set; }
    public decimal TotalPurchaseCost { get; set; }
    public decimal TotalSalesRevenue { get; set; }
    public decimal ProfitMargin => TotalSalesRevenue - (TotalPurchasedQuantity > 0 ? (TotalPurchaseCost / TotalPurchasedQuantity) * TotalSoldQuantity : 0);
    public bool IsActive { get; set; } = true;
}

public class CreateStationeryItemRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }

    [Required]
    [StringLength(30)]
    public string Unit { get; set; } = "Each"; // Each, Pack, Dozen, Box, Sheet, etc.

    [Range(0, 100000)]
    public decimal DefaultSellingPrice { get; set; }
}

public class UpdateStationeryItemRequest : CreateStationeryItemRequest
{
    public int Id { get; set; }
    public bool IsActive { get; set; } = true;
}

public class StationeryPurchaseDto
{
    public int Id { get; set; }
    public int StationeryItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime Date { get; set; }
    public string? Supplier { get; set; }
    public string? Notes { get; set; }
}

public class RecordStationeryPurchaseRequest
{
    [Required]
    public int StationeryItemId { get; set; }

    [Range(1, 100000)]
    public int Quantity { get; set; }

    [Range(0.01, 100000)]
    public decimal UnitCost { get; set; }

    public DateTime Date { get; set; } = DateTime.Today;

    [StringLength(100)]
    public string? Supplier { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}

public class StationerySaleDto
{
    public int Id { get; set; }
    public int StationeryItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime Date { get; set; }
    public int? StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? Notes { get; set; }
}

public class RecordStationerySaleRequest
{
    [Required]
    public int StationeryItemId { get; set; }

    [Range(1, 100000)]
    public int Quantity { get; set; }

    [Range(0.01, 100000)]
    public decimal UnitPrice { get; set; } // Flexible per-sale price override!

    public DateTime Date { get; set; } = DateTime.Today;

    public int? StudentId { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}

public class QuickStockAdjustmentRequest
{
    [Required]
    public int StationeryItemId { get; set; }

    [Range(0, 100000)]
    public int TargetStock { get; set; }

    public string Reason { get; set; } = "Inventory count correction";
}
