using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutionCenterMaintanance.Services.Interfaces;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Stationery;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StationeryController : ControllerBase
{
    private readonly IStationeryService _stationeryService;

    public StationeryController(IStationeryService stationeryService)
    {
        _stationeryService = stationeryService;
    }

    // ==========================================
    // 1. CATALOG ITEMS
    // ==========================================
    [HttpGet("items")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<StationeryItemDto>>>> GetItems(CancellationToken ct = default)
    {
        var items = await _stationeryService.GetItemsAsync(ct);
        return Ok(ApiResponse<List<StationeryItemDto>>.Ok(items));
    }

    [HttpPost("items")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<StationeryItemDto>>> CreateItem(
        [FromBody] CreateStationeryItemRequest request,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<StationeryItemDto>.Fail("Invalid item details"));

        var item = await _stationeryService.CreateItemAsync(request, ct);
        return CreatedAtAction(nameof(GetItems), new { id = item.Id }, ApiResponse<StationeryItemDto>.Ok(item, "Stationery item created"));
    }

    [HttpPut("items/{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateItem(
        int id,
        [FromBody] UpdateStationeryItemRequest request,
        CancellationToken ct = default)
    {
        if (id != request.Id)
            return BadRequest(ApiResponse<bool>.Fail("ID mismatch"));

        var updated = await _stationeryService.UpdateItemAsync(id, request, ct);
        if (!updated)
            return NotFound(ApiResponse<bool>.Fail("Stationery item not found."));

        return Ok(ApiResponse<bool>.Ok(true, "Stationery item updated"));
    }

    [HttpDelete("items/{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteItem(int id, CancellationToken ct = default)
    {
        var deleted = await _stationeryService.DeleteItemAsync(id, ct);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Stationery item not found."));

        return Ok(ApiResponse<bool>.Ok(true, "Stationery item deleted"));
    }

    // ==========================================
    // 2. STOCK PURCHASES (STOCK IN)
    // ==========================================
    [HttpGet("purchases")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<StationeryPurchaseDto>>>> GetPurchases(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        CancellationToken ct = default)
    {
        var result = await _stationeryService.GetPurchasesAsync(month, year, page, pageSize, ct);
        return Ok(ApiResponse<PaginatedResult<StationeryPurchaseDto>>.Ok(result));
    }

    [HttpPost("purchases")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<StationeryPurchaseDto>>> RecordPurchase(
        [FromBody] RecordStationeryPurchaseRequest request,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<StationeryPurchaseDto>.Fail("Invalid purchase details"));

        try
        {
            var purchase = await _stationeryService.RecordPurchaseAsync(request, ct);
            return CreatedAtAction(nameof(GetPurchases), new { id = purchase.Id }, ApiResponse<StationeryPurchaseDto>.Ok(purchase, "Stock purchase recorded"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<StationeryPurchaseDto>.Fail("Stationery item not found."));
        }
    }

    [HttpDelete("purchases/{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeletePurchase(int id, CancellationToken ct = default)
    {
        var deleted = await _stationeryService.DeletePurchaseAsync(id, ct);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Purchase record not found."));

        return Ok(ApiResponse<bool>.Ok(true, "Purchase record removed"));
    }

    // ==========================================
    // 3. SALES TO STUDENTS (STOCK OUT)
    // ==========================================
    [HttpGet("sales")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<StationerySaleDto>>>> GetSales(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        [FromQuery] int? studentId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        CancellationToken ct = default)
    {
        var result = await _stationeryService.GetSalesAsync(month, year, studentId, page, pageSize, ct);
        return Ok(ApiResponse<PaginatedResult<StationerySaleDto>>.Ok(result));
    }

    [HttpPost("sales")]
    [Authorize(Policy = Permissions.PaymentsRecord)]
    public async Task<ActionResult<ApiResponse<StationerySaleDto>>> RecordSale(
        [FromBody] RecordStationerySaleRequest request,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<StationerySaleDto>.Fail("Invalid sale details"));

        try
        {
            var sale = await _stationeryService.RecordSaleAsync(request, ct);
            return CreatedAtAction(nameof(GetSales), new { id = sale.Id }, ApiResponse<StationerySaleDto>.Ok(sale, "Sale recorded"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<StationerySaleDto>.Fail("Stationery item not found."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<StationerySaleDto>.Fail(ex.Message));
        }
    }

    [HttpDelete("sales/{id}")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteSale(int id, CancellationToken ct = default)
    {
        var deleted = await _stationeryService.DeleteSaleAsync(id, ct);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Sale record not found."));

        return Ok(ApiResponse<bool>.Ok(true, "Sale record removed"));
    }

    // ==========================================
    // 4. QUICK STOCK ADJUSTMENT
    // ==========================================
    [HttpPost("adjust-stock")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> AdjustStock(
        [FromBody] QuickStockAdjustmentRequest request,
        CancellationToken ct = default)
    {
        var adjusted = await _stationeryService.AdjustStockAsync(request, ct);
        if (!adjusted)
            return NotFound(ApiResponse<bool>.Fail("Stationery item not found."));

        return Ok(ApiResponse<bool>.Ok(true, "Stock level adjusted successfully."));
    }
}
