using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Harvests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/harvests")]
    public class HarvestsController : ControllerBase
    {
        private readonly IHarvestService _service;

        public HarvestsController(IHarvestService service) => _service = service;

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
        [HttpGet("plot/{plotId:guid}")]
        public async Task<IActionResult> GetByPlot(Guid plotId)
        {
            var result = await _service.GetByPlotIdAsync(plotId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet("season/{seasonId:guid}")]
        public async Task<IActionResult> GetBySeason(Guid seasonId)
        {
            var result = await _service.GetBySeasonIdAsync(seasonId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHarvestRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _service.CreateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHarvestRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
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
