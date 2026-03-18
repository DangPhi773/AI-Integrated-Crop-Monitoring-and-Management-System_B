using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Soils;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoilsController : ControllerBase
    {
        private readonly ISoilService _soilService;

        public SoilsController(ISoilService soilService) => _soilService = soilService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _soilService.GetAllSoilsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _soilService.GetSoilByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SoilRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _soilService.CreateSoilAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SoilRequest request)
        {
            var result = await _soilService.UpdateSoilAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _soilService.DeleteSoilAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
