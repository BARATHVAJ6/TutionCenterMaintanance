using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Services.Interfaces;
using TutionCenterMaintanance.Shared.DTOs.Stationery;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Services;

public class StationeryService : IStationeryService
{
    private readonly AppDbContext _context;

    public StationeryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StationeryItemDto>> GetItemsAsync(CancellationToken ct = default)
    {
        var items = await _context.StationeryItems
            .AsNoTracking()
            .Include(i => i.Purchases.Where(p => !p.IsDeleted))
            .Include(i => i.Sales.Where(s => !s.IsDeleted))
            .Where(i => i.IsActive && !i.IsDeleted)
            .OrderBy(i => i.Name)
            .ToListAsync(ct);

        return items.Select(i =>
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
    }

    public async Task<StationeryItemDto> CreateItemAsync(CreateStationeryItemRequest request, CancellationToken ct = default)
    {
        var item = new StationeryItem
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Unit = !string.IsNullOrWhiteSpace(request.Unit) ? request.Unit.Trim() : "Each",
            DefaultSellingPrice = request.DefaultSellingPrice,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.StationeryItems.Add(item);
        await _context.SaveChangesAsync(ct);

        return new StationeryItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Unit = item.Unit,
            DefaultSellingPrice = item.DefaultSellingPrice,
            CurrentStock = 0,
            IsActive = true
        };
    }

    public async Task<bool> UpdateItemAsync(int id, UpdateStationeryItemRequest request, CancellationToken ct = default)
    {
        var item = await _context.StationeryItems.FindAsync(new object[] { id }, ct);
        if (item == null || item.IsDeleted) return false;

        item.Name = request.Name.Trim();
        item.Description = request.Description?.Trim();
        item.Unit = !string.IsNullOrWhiteSpace(request.Unit) ? request.Unit.Trim() : "Each";
        item.DefaultSellingPrice = request.DefaultSellingPrice;
        item.IsActive = request.IsActive;
        item.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteItemAsync(int id, CancellationToken ct = default)
    {
        var item = await _context.StationeryItems.FindAsync(new object[] { id }, ct);
        if (item == null || item.IsDeleted) return false;

        item.IsDeleted = true;
        item.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<PaginatedResult<StationeryPurchaseDto>> GetPurchasesAsync(
        int? month = null,
        int? year = null,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default)
    {
        pageSize = Math.Clamp(pageSize, 1, 100);
        page = Math.Max(1, page);

        var query = _context.StationeryPurchases
            .AsNoTracking()
            .Include(p => p.StationeryItem)
            .Where(p => !p.IsDeleted)
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

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.Date)
            .ThenByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new StationeryPurchaseDto
            {
                Id = p.Id,
                StationeryItemId = p.StationeryItemId,
                ItemName = p.StationeryItem != null ? p.StationeryItem.Name : string.Empty,
                Unit = p.StationeryItem != null ? p.StationeryItem.Unit : string.Empty,
                Quantity = p.Quantity,
                UnitCost = p.UnitCost,
                TotalCost = p.TotalCost,
                Date = p.Date,
                Supplier = p.Supplier,
                Notes = p.Notes
            })
            .ToListAsync(ct);

        return new PaginatedResult<StationeryPurchaseDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<StationeryPurchaseDto> RecordPurchaseAsync(RecordStationeryPurchaseRequest request, CancellationToken ct = default)
    {
        var item = await _context.StationeryItems.FindAsync(new object[] { request.StationeryItemId }, ct);
        if (item == null || item.IsDeleted)
            throw new KeyNotFoundException("Stationery item not found.");

        var totalCost = Math.Round(request.Quantity * request.UnitCost, 2);

        var purchase = new StationeryPurchase
        {
            StationeryItemId = request.StationeryItemId,
            Quantity = request.Quantity,
            UnitCost = request.UnitCost,
            TotalCost = totalCost,
            Date = request.Date.Date != default ? request.Date : DateTime.Today,
            Supplier = request.Supplier?.Trim(),
            Notes = request.Notes?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.StationeryPurchases.Add(purchase);
        await _context.SaveChangesAsync(ct);

        return new StationeryPurchaseDto
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
    }

    public async Task<bool> DeletePurchaseAsync(int id, CancellationToken ct = default)
    {
        var purchase = await _context.StationeryPurchases.FindAsync(new object[] { id }, ct);
        if (purchase == null || purchase.IsDeleted) return false;

        purchase.IsDeleted = true;
        purchase.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<PaginatedResult<StationerySaleDto>> GetSalesAsync(
        int? month = null,
        int? year = null,
        int? studentId = null,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default)
    {
        pageSize = Math.Clamp(pageSize, 1, 100);
        page = Math.Max(1, page);

        var query = _context.StationerySales
            .AsNoTracking()
            .Include(s => s.StationeryItem)
            .Include(s => s.Student)
            .Where(s => !s.IsDeleted)
            .AsQueryable();

        if (studentId.HasValue)
        {
            query = query.Where(s => s.StudentId == studentId.Value);
        }

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

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(s => s.Date)
            .ThenByDescending(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new StationerySaleDto
            {
                Id = s.Id,
                StationeryItemId = s.StationeryItemId,
                ItemName = s.StationeryItem != null ? s.StationeryItem.Name : string.Empty,
                Unit = s.StationeryItem != null ? s.StationeryItem.Unit : string.Empty,
                Quantity = s.Quantity,
                UnitPrice = s.UnitPrice,
                TotalAmount = s.TotalAmount,
                Date = s.Date,
                StudentId = s.StudentId,
                StudentName = s.Student != null ? $"{s.Student.FirstName} {s.Student.LastName}".Trim() : null,
                Notes = s.Notes
            })
            .ToListAsync(ct);

        return new PaginatedResult<StationerySaleDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<StationerySaleDto> RecordSaleAsync(RecordStationerySaleRequest request, CancellationToken ct = default)
    {
        var item = await _context.StationeryItems
            .Include(i => i.Purchases.Where(p => !p.IsDeleted))
            .Include(i => i.Sales.Where(s => !s.IsDeleted))
            .FirstOrDefaultAsync(i => i.Id == request.StationeryItemId, ct);

        if (item == null || item.IsDeleted)
            throw new KeyNotFoundException("Stationery item not found.");

        var currentStock = item.Purchases.Sum(p => p.Quantity) - item.Sales.Sum(s => s.Quantity);
        if (request.Quantity > currentStock)
        {
            throw new InvalidOperationException($"Insufficient stock. Requested: {request.Quantity}, Available: {Math.Max(0, currentStock)}.");
        }

        var totalAmount = Math.Round(request.Quantity * request.UnitPrice, 2);

        var sale = new StationerySale
        {
            StationeryItemId = request.StationeryItemId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            TotalAmount = totalAmount,
            Date = request.Date.Date != default ? request.Date : DateTime.Today,
            StudentId = request.StudentId,
            Notes = request.Notes?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.StationerySales.Add(sale);
        await _context.SaveChangesAsync(ct);

        string? studentName = null;
        if (request.StudentId.HasValue)
        {
            var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == request.StudentId.Value, ct);
            if (student != null)
            {
                studentName = $"{student.FirstName} {student.LastName}".Trim();
            }
        }

        return new StationerySaleDto
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
    }

    public async Task<bool> DeleteSaleAsync(int id, CancellationToken ct = default)
    {
        var sale = await _context.StationerySales.FindAsync(new object[] { id }, ct);
        if (sale == null || sale.IsDeleted) return false;

        sale.IsDeleted = true;
        sale.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> AdjustStockAsync(QuickStockAdjustmentRequest request, CancellationToken ct = default)
    {
        var item = await _context.StationeryItems
            .Include(i => i.Purchases.Where(p => !p.IsDeleted))
            .Include(i => i.Sales.Where(s => !s.IsDeleted))
            .FirstOrDefaultAsync(i => i.Id == request.StationeryItemId, ct);

        if (item == null || item.IsDeleted) return false;

        var currentStock = item.Purchases.Sum(p => p.Quantity) - item.Sales.Sum(s => s.Quantity);
        var diff = request.TargetStock - currentStock;

        if (diff == 0) return true;

        if (diff > 0)
        {
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

        await _context.SaveChangesAsync(ct);
        return true;
    }
}
