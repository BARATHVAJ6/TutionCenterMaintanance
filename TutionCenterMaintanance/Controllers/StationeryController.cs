using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Stationery;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StationeryController : ControllerBase
{
    private readonly AppDbContext _context;

    public StationeryController(AppDbContext context)
    {
        _context = context;
    }

    // ==========================================
    // 1. CATALOG ITEMS
    // ==========================================
    [HttpGet("items")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<StationeryItemDto>>>> GetItems()
    {
        var items = await _context.StationeryItems
            .Include(i => i.Purchases)
            .Include(i => i.Sales)
            .Where(i => i.IsActive)
            .OrderBy(i => i.Name)
            .ToListAsync();

        var list = items.Select(i =>
        {
            var purchasedQty = i.Purchases.Sum(p => p.Quantity);
            var soldQty = i.Sales.Sum(s => s.Quantity);
            var purchaseCost = i.Purchases.Sum(p => p.TotalCost);
            var salesRev = i.Sales.Sum(s => s.TotalAmount);
            var currentStock = Math.Max(0, purchasedQty - soldQty);

            return new StationeryItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Unit = i.Unit,
                DefaultSellingPrice = i.DefaultSellingPrice,
                CurrentStock = currentStock,
                TotalPurchasedQuantity = purchasedQty,
                TotalSoldQuantity = soldQty,
                TotalPurchaseCost = purchaseCost,
                TotalSalesRevenue = salesRev,
                IsActive = i.IsActive
            };
        }).ToList();

        return Ok(ApiResponse<List<StationeryItemDto>>.Ok(list));
    }

    [HttpPost("items")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<StationeryItemDto>>> CreateItem([FromBody] CreateStationeryItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest(ApiResponse<StationeryItemDto>.Fail("Item name is required."));

        var item = new StationeryItem
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Unit = string.IsNullOrWhiteSpace(request.Unit) ? "Each" : request.Unit.Trim(),
            DefaultSellingPrice = request.DefaultSellingPrice,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.StationeryItems.Add(item);
        await _context.SaveChangesAsync();

        var dto = new StationeryItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Unit = item.Unit,
            DefaultSellingPrice = item.DefaultSellingPrice,
            CurrentStock = 0,
            IsActive = true
        };

        return Ok(ApiResponse<StationeryItemDto>.Ok(dto, "Stationery item created successfully"));
    }

    [HttpPut("items/{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateItem(int id, [FromBody] UpdateStationeryItemRequest request)
    {
        var item = await _context.StationeryItems.FindAsync(id);
        if (item == null)
            return NotFound(ApiResponse<bool>.Fail("Stationery item not found."));

        item.Name = request.Name.Trim();
        item.Description = request.Description?.Trim();
        item.Unit = string.IsNullOrWhiteSpace(request.Unit) ? "Each" : request.Unit.Trim();
        item.DefaultSellingPrice = request.DefaultSellingPrice;
        item.IsActive = request.IsActive;
        item.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Stationery item updated"));
    }

    [HttpDelete("items/{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteItem(int id)
    {
        var item = await _context.StationeryItems.FindAsync(id);
        if (item == null)
            return NotFound(ApiResponse<bool>.Fail("Stationery item not found."));

        item.IsDeleted = true;
        item.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Stationery item deleted"));
    }

    // ==========================================
    // 2. STOCK PURCHASES (STOCK IN)
    // ==========================================
    [HttpGet("purchases")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<StationeryPurchaseDto>>>> GetPurchases(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null)
    {
        var query = _context.StationeryPurchases
            .Include(p => p.StationeryItem)
            .AsQueryable();

        if (month.HasValue && year.HasValue)
        {
            var start = new DateTime(year.Value, month.Value, 1);
            var end = start.AddMonths(1).AddTicks(-1);
            query = query.Where(p => p.Date >= start && p.Date <= end);
        }
        else if (year.HasValue)
        {
            var start = new DateTime(year.Value, 1, 1);
            var end = new DateTime(year.Value, 12, 31, 23, 59, 59);
            query = query.Where(p => p.Date >= start && p.Date <= end);
        }

        var purchases = await query
            .OrderByDescending(p => p.Date)
            .ThenByDescending(p => p.Id)
            .Select(p => new StationeryPurchaseDto
            {
                Id = p.Id,
                StationeryItemId = p.StationeryItemId,
                ItemName = p.StationeryItem.Name,
                Unit = p.StationeryItem.Unit,
                Quantity = p.Quantity,
                UnitCost = p.UnitCost,
                TotalCost = p.TotalCost,
                Date = p.Date,
                Supplier = p.Supplier,
                Notes = p.Notes
            })
            .ToListAsync();

        return Ok(ApiResponse<List<StationeryPurchaseDto>>.Ok(purchases));
    }

    [HttpPost("purchases")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<StationeryPurchaseDto>>> RecordPurchase([FromBody] RecordStationeryPurchaseRequest request)
    {
        if (request.Quantity <= 0)
            return BadRequest(ApiResponse<StationeryPurchaseDto>.Fail("Quantity must be greater than zero."));

        if (request.UnitCost < 0)
            return BadRequest(ApiResponse<StationeryPurchaseDto>.Fail("Unit cost cannot be negative."));

        var item = await _context.StationeryItems.FindAsync(request.StationeryItemId);
        if (item == null)
            return NotFound(ApiResponse<StationeryPurchaseDto>.Fail("Stationery item not found."));

        var purchase = new StationeryPurchase
        {
            StationeryItemId = request.StationeryItemId,
            Quantity = request.Quantity,
            UnitCost = request.UnitCost,
            TotalCost = request.Quantity * request.UnitCost,
            Date = request.Date.Date != default ? request.Date : DateTime.Today,
            Supplier = request.Supplier?.Trim(),
            Notes = request.Notes?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.StationeryPurchases.Add(purchase);
        await _context.SaveChangesAsync();

        var dto = new StationeryPurchaseDto
        {
            Id = purchase.Id,
            StationeryItemId = purchase.StationeryItemId,
            ItemName = item.Name,
            Unit = item.Unit,
            Quantity = purchase.Quantity,
            UnitCost = purchase.UnitCost,
            TotalCost = purchase.TotalCost,
            Date = purchase.Date,
            Supplier = purchase.Supplier,
            Notes = purchase.Notes
        };

        return Ok(ApiResponse<StationeryPurchaseDto>.Ok(dto, "Stock purchase recorded successfully"));
    }

    [HttpDelete("purchases/{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeletePurchase(int id)
    {
        var purchase = await _context.StationeryPurchases.FindAsync(id);
        if (purchase == null)
            return NotFound(ApiResponse<bool>.Fail("Purchase record not found."));

        purchase.IsDeleted = true;
        purchase.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Purchase record deleted"));
    }

    // ==========================================
    // 3. SALES TO STUDENTS (STOCK OUT & REVENUE)
    // ==========================================
    [HttpGet("sales")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<StationerySaleDto>>>> GetSales(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null)
    {
        var query = _context.StationerySales
            .Include(s => s.StationeryItem)
            .Include(s => s.Student)
            .AsQueryable();

        if (month.HasValue && year.HasValue)
        {
            var start = new DateTime(year.Value, month.Value, 1);
            var end = start.AddMonths(1).AddTicks(-1);
            query = query.Where(s => s.Date >= start && s.Date <= end);
        }
        else if (year.HasValue)
        {
            var start = new DateTime(year.Value, 1, 1);
            var end = new DateTime(year.Value, 12, 31, 23, 59, 59);
            query = query.Where(s => s.Date >= start && s.Date <= end);
        }

        var sales = await query
            .OrderByDescending(s => s.Date)
            .ThenByDescending(s => s.Id)
            .Select(s => new StationerySaleDto
            {
                Id = s.Id,
                StationeryItemId = s.StationeryItemId,
                ItemName = s.StationeryItem.Name,
                Unit = s.StationeryItem.Unit,
                Quantity = s.Quantity,
                UnitPrice = s.UnitPrice,
                TotalAmount = s.TotalAmount,
                Date = s.Date,
                StudentId = s.StudentId,
                StudentName = s.Student != null ? $"{s.Student.FirstName} {s.Student.LastName}".Trim() : null,
                Notes = s.Notes
            })
            .ToListAsync();

        return Ok(ApiResponse<List<StationerySaleDto>>.Ok(sales));
    }

    [HttpPost("sales")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<StationerySaleDto>>> RecordSale([FromBody] RecordStationerySaleRequest request)
    {
        if (request.Quantity <= 0)
            return BadRequest(ApiResponse<StationerySaleDto>.Fail("Quantity must be greater than zero."));

        if (request.UnitPrice <= 0)
            return BadRequest(ApiResponse<StationerySaleDto>.Fail("Unit price must be greater than zero."));

        var item = await _context.StationeryItems
            .Include(i => i.Purchases)
            .Include(i => i.Sales)
            .FirstOrDefaultAsync(i => i.Id == request.StationeryItemId);

        if (item == null)
            return NotFound(ApiResponse<StationerySaleDto>.Fail("Stationery item not found."));

        var currentStock = item.Purchases.Sum(p => p.Quantity) - item.Sales.Sum(s => s.Quantity);
        if (request.Quantity > currentStock)
        {
            return BadRequest(ApiResponse<StationerySaleDto>.Fail(
                $"Insufficient stock! Available: {currentStock} {item.Unit}(s), requested: {request.Quantity}."));
        }

        var sale = new StationerySale
        {
            StationeryItemId = request.StationeryItemId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice, // Flexible per-sale price!
            TotalAmount = request.Quantity * request.UnitPrice,
            Date = request.Date.Date != default ? request.Date : DateTime.Today,
            StudentId = request.StudentId,
            Notes = request.Notes?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.StationerySales.Add(sale);
        await _context.SaveChangesAsync();

        string? studentName = null;
        if (request.StudentId.HasValue)
        {
            var st = await _context.Students.FindAsync(request.StudentId.Value);
            if (st != null) studentName = $"{st.FirstName} {st.LastName}".Trim();
        }

        var dto = new StationerySaleDto
        {
            Id = sale.Id,
            StationeryItemId = sale.StationeryItemId,
            ItemName = item.Name,
            Unit = item.Unit,
            Quantity = sale.Quantity,
            UnitPrice = sale.UnitPrice,
            TotalAmount = sale.TotalAmount,
            Date = sale.Date,
            StudentId = sale.StudentId,
            StudentName = studentName,
            Notes = sale.Notes
        };

        return Ok(ApiResponse<StationerySaleDto>.Ok(dto, "Sale recorded successfully"));
    }

    [HttpDelete("sales/{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteSale(int id)
    {
        var sale = await _context.StationerySales.FindAsync(id);
        if (sale == null)
            return NotFound(ApiResponse<bool>.Fail("Sale record not found."));

        sale.IsDeleted = true;
        sale.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Sale record reversed"));
    }

    // ==========================================
    // 4. FLEXIBLE STOCK ADJUSTMENT
    // ==========================================
    [HttpPost("adjust-stock")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> AdjustStock([FromBody] QuickStockAdjustmentRequest request)
    {
        var item = await _context.StationeryItems
            .Include(i => i.Purchases)
            .Include(i => i.Sales)
            .FirstOrDefaultAsync(i => i.Id == request.StationeryItemId);

        if (item == null)
            return NotFound(ApiResponse<bool>.Fail("Stationery item not found."));

        var currentStock = item.Purchases.Sum(p => p.Quantity) - item.Sales.Sum(s => s.Quantity);
        var diff = request.TargetStock - currentStock;

        if (diff == 0)
        {
            return Ok(ApiResponse<bool>.Ok(true, "Stock level is already at target."));
        }

        if (diff > 0)
        {
            // Stock increase -> add a Purchase record with 0 cost or default cost
            _context.StationeryPurchases.Add(new StationeryPurchase
            {
                StationeryItemId = item.Id,
                Quantity = diff,
                UnitCost = 0,
                TotalCost = 0,
                Date = DateTime.Today,
                Supplier = "Stock Count Adjustment (+)",
                Notes = request.Reason,
                CreatedAt = DateTime.UtcNow
            });
        }
        else
        {
            // Stock decrease -> add a Sale record with 0 price to balance inventory
            _context.StationerySales.Add(new StationerySale
            {
                StationeryItemId = item.Id,
                Quantity = Math.Abs(diff),
                UnitPrice = 0,
                TotalAmount = 0,
                Date = DateTime.Today,
                Notes = $"Adjustment (-): {request.Reason}",
                CreatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, $"Stock adjusted to {request.TargetStock} {item.Unit}(s)."));
    }
}
