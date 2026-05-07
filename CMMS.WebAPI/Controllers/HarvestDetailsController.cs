using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Harvests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/harvest-details")]
    public class HarvestDetailsController : ControllerBase
    {
        private readonly IHarvestDetailService _service;

        public HarvestDetailsController(IHarvestDetailService service) => _service = service;

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet("harvest/{harvestId:guid}")]
        public async Task<IActionResult> GetByHarvest(Guid harvestId)
        {
            var result = await _service.GetByHarvestIdAsync(harvestId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHarvestDetailRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpPost("{id:guid}/record-harvest")]
        public async Task<IActionResult> RecordHarvest(Guid id, [FromBody] RecordHarvestRequest request)
        {
            var result = await _service.RecordHarvestAsync(id, request);
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
