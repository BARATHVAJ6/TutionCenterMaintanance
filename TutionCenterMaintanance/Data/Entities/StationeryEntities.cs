namespace TutionCenterMaintanance.Data.Entities;

public class StationeryItem : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Unit { get; set; } = "Each"; // Each, Pack, Box, Dozen, etc.
    public decimal DefaultSellingPrice { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    public ICollection<StationeryPurchase> Purchases { get; set; } = new List<StationeryPurchase>();
    public ICollection<StationerySale> Sales { get; set; } = new List<StationerySale>();
}

public class StationeryPurchase : BaseEntity
{
    public int StationeryItemId { get; set; }
    public StationeryItem StationeryItem { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Supplier { get; set; }
    public string? Notes { get; set; }
}

public class StationerySale : BaseEntity
{
    public int StationeryItemId { get; set; }
    public StationeryItem StationeryItem { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int? StudentId { get; set; }
    public Student? Student { get; set; }

    public string? Notes { get; set; }
}
