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

    public PaymentController(IDiagnosisBillingService billingService)
    {
        _billingService = billingService;
    }

    [HttpGet("bill")]
    [Authorize(Policy = "SpecialistOnly")]
    public async Task<IActionResult> GetBill([FromQuery] Guid specialistId, [FromQuery] DateTime month)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var result = await _billingService.GetBillForMonthAsync(specialistId, month, userId, role);
        return Ok(result);
    }

    [HttpPost("upload")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Upload([FromForm] UploadPaymentRequest request)
    {
        var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _billingService.UploadPaymentAsync(request, ownerId);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize(Policy = "SpecialistOnly")]
    public async Task<IActionResult> GetMy()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var result = await _billingService.GetMyPaymentsAsync(userId, role);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "SpecialistOnly")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var result = await _billingService.GetPaymentByIdAsync(id, userId, role);
        return Ok(result);
    }
}
