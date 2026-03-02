using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Farms;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmsController : ControllerBase
    {
        private readonly IFarmService _farmService;

        public FarmsController(IFarmService farmService) => _farmService = farmService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _farmService.GetAllFarmsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _farmService.GetFarmByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FarmRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _farmService.CreateFarmAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] FarmRequest request)
        {
            var result = await _farmService.UpdateFarmAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _farmService.DeleteFarmAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
