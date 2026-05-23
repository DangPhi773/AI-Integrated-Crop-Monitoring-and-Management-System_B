using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMMS.WebAPI.Controllers;

[ApiController]
[Route("api/contract")]
public class ContractController : ControllerBase
{
    private readonly IDiagnosisContractService _contractService;

    public ContractController(IDiagnosisContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpPost]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest request)
    {
        var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _contractService.CreateAsync(request, ownerId);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize(Policy = "SpecialistOnly")]
    public async Task<IActionResult> GetMy()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var result = await _contractService.GetMyContractsAsync(userId, role);
        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _contractService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "SpecialistOnly")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var result = await _contractService.GetByIdAsync(id, userId, role);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContractRequest request)
    {
        var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _contractService.UpdateAsync(id, request, ownerId);
        return Ok(result);
    }

    [HttpPost("{id:guid}/terminate")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Terminate(Guid id)
    {
        var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _contractService.TerminateAsync(id, ownerId);
        return Ok(result);
    }
}
