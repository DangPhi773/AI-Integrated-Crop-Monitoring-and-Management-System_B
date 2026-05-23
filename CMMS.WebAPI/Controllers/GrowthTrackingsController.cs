using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.GrowthTrackings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/growth-trackings")]
    public class GrowthTrackingsController : ControllerBase
    {
        private readonly IGrowthTrackingService _service;

        public GrowthTrackingsController(IGrowthTrackingService service) => _service = service;

        private Guid? GetUserId()
        {
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            return Guid.TryParse(raw, out var id) ? id : null;
        }

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet("harvest-detail/{harvestDetailId:guid}")]
        public async Task<IActionResult> GetByHarvestDetail(Guid harvestDetailId)
        {
            var result = await _service.GetByHarvestDetailIdAsync(harvestDetailId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Specialist")]
        [HttpGet("season/{seasonId:guid}/progress")]
        public async Task<IActionResult> GetSeasonProgress(Guid seasonId)
        {
            var result = await _service.GetSeasonProgressAsync(seasonId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GrowthTrackingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _service.CreateAsync(request, GetUserId());
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] GrowthTrackingUpdateRequest request)
        {
            var result = await _service.UpdateAsync(id, request, GetUserId());
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpPost("{id:guid}/advance")]
        public async Task<IActionResult> Advance(Guid id, [FromBody] AdvanceStageRequest request)
        {
            var result = await _service.AdvanceStageAsync(id, request, GetUserId());
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var result = await _service.CompleteAsync(id, GetUserId());
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
