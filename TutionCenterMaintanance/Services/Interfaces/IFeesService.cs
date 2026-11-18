using TutionCenterMaintanance.Shared.DTOs.Fees;

namespace TutionCenterMaintanance.Services.Interfaces;

public interface IFeesService
{
    Task<List<StudentMonthlyStatusDto>> GetMonthlyStatusAsync(int year, int month, int? gradeId = null, CancellationToken ct = default);

    Task<bool> SetMonthlyFeeAsync(SetMonthlyFeeRequest request, CancellationToken ct = default);

    Task<PaymentDto> RecordPaymentAsync(RecordPaymentRequest request, string? currentUserId, CancellationToken ct = default);

    Task<List<PaymentDto>> GetPaymentsAsync(int? studentId = null, CancellationToken ct = default);

    Task<PaymentReceiptDto?> GetPaymentReceiptAsync(int paymentId, CancellationToken ct = default);

    Task<StudentFeeLedgerDto?> GetStudentFeeLedgerAsync(int studentId, CancellationToken ct = default);
}
