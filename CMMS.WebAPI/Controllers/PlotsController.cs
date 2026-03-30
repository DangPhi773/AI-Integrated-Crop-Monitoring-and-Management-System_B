using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Plots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize (Roles = "Owner")]
    [ApiController]
    [Route("api/[controller]")]
    public class PlotsController : ControllerBase
    {
        private readonly IPlotService _plotService;

        public PlotsController(IPlotService plotService) => _plotService = plotService;

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _plotService.GetAllPlotsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _plotService.GetPlotByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlotRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _plotService.CreatePlotAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PlotRequest request)
        {
            var result = await _plotService.UpdatePlotAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _plotService.DeletePlotAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
