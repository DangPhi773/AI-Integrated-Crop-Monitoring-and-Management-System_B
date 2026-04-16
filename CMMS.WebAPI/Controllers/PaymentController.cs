using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMMS.WebAPI.Controllers;

[ApiController]
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly IDiagnosisBillingService _billingService;
    private readonly IConfiguration _config;

    public PaymentController(IDiagnosisBillingService billingService, IConfiguration config)
    {
        _billingService = billingService;
        _config = config;
    }

    [HttpPost("price-setting")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> CreatePriceSetting([FromBody] CreatePriceSettingRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _billingService.CreatePriceSettingAsync(request, userId);
        return Ok(result);
    }

    [HttpGet("bill/{priceSettingId}")]
    [Authorize(Policy = "SpecialistOnly")]
    public async Task<IActionResult> GetBillInfo(Guid priceSettingId)
    {
        var result = await _billingService.GetBillInfoAsync(priceSettingId);
        return Ok(result);
    }

    [HttpGet("bill")]
    [Authorize(Policy = "SpecialistOnly")]
    public async Task<IActionResult> GetBillByParams(
        [FromQuery] Guid farmId,
        [FromQuery] Guid expertId,
        [FromQuery] DateTime month)
    {
        var result = await _billingService.GetBillInfoByParamsAsync(farmId, expertId, month);
        return Ok(result);
    }

    [HttpPost("create")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
    {
        var result = await _billingService.CreatePaymentAsync(request);
        return Ok(result);
    }

    [HttpGet("vnpay-return")]
    [AllowAnonymous]
    public async Task<IActionResult> VNPayReturn()
    {
        await _billingService.ProcessPaymentCallbackAsync("vnpay", Request.Query);

        var paymentId = Request.Query["vnp_TxnRef"].ToString();
        var status = Request.Query["vnp_ResponseCode"].ToString() == "00" ? "success" : "failed";

        var frontendUrl = _config["FrontendUrl"] ?? "http://localhost:3000";
        return Redirect($"{frontendUrl}/payment/result?id={paymentId}&status={status}");
    }

    [HttpGet("payos-return")]
    [AllowAnonymous]
    public async Task<IActionResult> PayOSReturn()
    {
        await _billingService.ProcessPaymentCallbackAsync("payos", Request.Query);

        var orderCode = Request.Query["orderCode"].ToString();
        var status = Request.Query["status"].ToString() == "PAID" ? "success" : "failed";

        var frontendUrl = _config["FrontendUrl"] ?? "http://localhost:3000";
        return Redirect($"{frontendUrl}/payment/result?id={orderCode}&status={status}");
    }

    [HttpGet("payos-cancel")]
    [AllowAnonymous]
    public IActionResult PayOSCancel()
    {
        var frontendUrl = _config["FrontendUrl"] ?? "http://localhost:3000";
        return Redirect($"{frontendUrl}/payment/cancelled");
    }
}
