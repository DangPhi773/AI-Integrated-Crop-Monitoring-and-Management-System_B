using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Beds;
using CMMS.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Thêm thư viện này để đọc Claims

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
     [Authorize]
    public class BedsController : ControllerBase
    {
        private readonly IBedService _bedService;

        public BedsController(IBedService bedService) => _bedService = bedService;

        [Authorize(Roles = "Owner,Worker,Specialist")] 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bedService.GetAllBedsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Specialist")] 
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _bedService.GetBedByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BedRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _bedService.CreateBedAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BedRequest request)
        {
            var result = await _bedService.UpdateBedAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _bedService.DeleteBedAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("auto-allocate/preview")]
        public async Task<IActionResult> PreviewAutoAllocate([FromBody] BedSplitRequest request)
        {
            var result = await _bedService.PreviewAutoAllocateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("auto-allocate/confirm")]
        public async Task<IActionResult> ConfirmAutoAllocate([FromBody] BedSplitConfirmRequest request)
        {
            var result = await _bedService.ConfirmAutoAllocateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Specialist")]
        [HttpGet("plot/{plotId}")]
        public async Task<IActionResult> GetBedsByPlot(Guid plotId)
        {
            var result = await _bedService.GetBedsByPlotIdAsync(plotId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}