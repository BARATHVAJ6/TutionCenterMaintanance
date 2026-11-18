using TutionCenterMaintanance.Shared.DTOs.Stationery;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Services.Interfaces;

public interface IStationeryService
{
    Task<List<StationeryItemDto>> GetItemsAsync(CancellationToken ct = default);

    Task<StationeryItemDto> CreateItemAsync(CreateStationeryItemRequest request, CancellationToken ct = default);

    Task<bool> UpdateItemAsync(int id, UpdateStationeryItemRequest request, CancellationToken ct = default);

    Task<bool> DeleteItemAsync(int id, CancellationToken ct = default);

    Task<PaginatedResult<StationeryPurchaseDto>> GetPurchasesAsync(
        int? month = null,
        int? year = null,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default);

    Task<StationeryPurchaseDto> RecordPurchaseAsync(RecordStationeryPurchaseRequest request, CancellationToken ct = default);

    Task<bool> DeletePurchaseAsync(int id, CancellationToken ct = default);

    Task<PaginatedResult<StationerySaleDto>> GetSalesAsync(
        int? month = null,
        int? year = null,
        int? studentId = null,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default);

    Task<StationerySaleDto> RecordSaleAsync(RecordStationerySaleRequest request, CancellationToken ct = default);

    Task<bool> DeleteSaleAsync(int id, CancellationToken ct = default);

    Task<bool> AdjustStockAsync(QuickStockAdjustmentRequest request, CancellationToken ct = default);
}
