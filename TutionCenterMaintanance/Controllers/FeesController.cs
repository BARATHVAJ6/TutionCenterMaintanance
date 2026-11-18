using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutionCenterMaintanance.Services.Interfaces;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Fees;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeesController : ControllerBase
{
    private readonly IFeesService _feesService;

    public FeesController(IFeesService feesService)
    {
        _feesService = feesService;
    }

    // --- Monthly Student Status (Main grid for 30-40 students) ---
    [HttpGet("monthly-status")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<List<StudentMonthlyStatusDto>>>> GetMonthlyStatus(
        [FromQuery] int year,
        [FromQuery] int month,
        [FromQuery] int? gradeId = null,
        CancellationToken ct = default)
    {
        if (year < 2000 || year > 2100) year = DateTime.Today.Year;
        if (month < 1 || month > 12) month = DateTime.Today.Month;

        var list = await _feesService.GetMonthlyStatusAsync(year, month, gradeId, ct);
        return Ok(ApiResponse<List<StudentMonthlyStatusDto>>.Ok(list));
    }

    // --- Set / Update Student Monthly Fee ---
    [HttpPut("monthly-fee")]
    [Authorize(Policy = Permissions.FeesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> SetMonthlyFee(
        [FromBody] SetMonthlyFeeRequest request,
        CancellationToken ct = default)
    {
        if (request.StudentId <= 0)
            return BadRequest(ApiResponse<bool>.Fail("Invalid student ID."));

        if (request.MonthlyFeeAmount < 0)
            return BadRequest(ApiResponse<bool>.Fail("Fee amount cannot be negative."));

        var success = await _feesService.SetMonthlyFeeAsync(request, ct);
        if (!success)
            return NotFound(ApiResponse<bool>.Fail("Student not found."));

        return Ok(ApiResponse<bool>.Ok(true, "Monthly fee updated successfully"));
    }

    // --- Collect Fee Payment (Cash, UPI, etc.) ---
    [HttpPost("pay")]
    [Authorize(Policy = Permissions.PaymentsRecord)]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> RecordPayment(
        [FromBody] RecordPaymentRequest request,
        CancellationToken ct = default)
    {
        if (request.StudentId <= 0)
            return BadRequest(ApiResponse<PaymentDto>.Fail("Invalid student ID"));

        if (request.Amount <= 0)
            return BadRequest(ApiResponse<PaymentDto>.Fail("Payment amount must be greater than zero"));

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var paymentDto = await _feesService.RecordPaymentAsync(request, currentUserId, ct);
            return CreatedAtAction(nameof(GetPaymentReceipt), new { paymentId = paymentDto.Id }, ApiResponse<PaymentDto>.Ok(paymentDto, "Payment recorded successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<PaymentDto>.Fail("Student not found"));
        }
    }

    // --- List Payments ---
    [HttpGet("payments")]
    [Authorize(Policy = Permissions.PaymentsView)]
    public async Task<ActionResult<ApiResponse<List<PaymentDto>>>> GetPayments(
        [FromQuery] int? studentId = null,
        CancellationToken ct = default)
    {
        var payments = await _feesService.GetPaymentsAsync(studentId, ct);
        return Ok(ApiResponse<List<PaymentDto>>.Ok(payments));
    }

    // --- Official Payment Receipt ---
    [HttpGet("receipt/{paymentId}")]
    [Authorize(Policy = Permissions.PaymentsView)]
    public async Task<ActionResult<ApiResponse<PaymentReceiptDto>>> GetPaymentReceipt(
        int paymentId,
        CancellationToken ct = default)
    {
        var receipt = await _feesService.GetPaymentReceiptAsync(paymentId, ct);
        if (receipt == null)
            return NotFound(ApiResponse<PaymentReceiptDto>.Fail("Payment record not found"));

        return Ok(ApiResponse<PaymentReceiptDto>.Ok(receipt));
    }

    // --- Student Fee Ledger (History) ---
    [HttpGet("ledger/{studentId}")]
    [Authorize(Policy = Permissions.FeesView)]
    public async Task<ActionResult<ApiResponse<StudentFeeLedgerDto>>> GetStudentFeeLedger(
        int studentId,
        CancellationToken ct = default)
    {
        var ledger = await _feesService.GetStudentFeeLedgerAsync(studentId, ct);
        if (ledger == null)
            return NotFound(ApiResponse<StudentFeeLedgerDto>.Fail("Student not found"));

        return Ok(ApiResponse<StudentFeeLedgerDto>.Ok(ledger));
    }
}
