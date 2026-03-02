using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Seasons;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeasonsController : ControllerBase
    {
        private readonly ISeasonService _seasonService;

        public SeasonsController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _seasonService.GetAllSeasonsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _seasonService.GetSeasonByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SeasonRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _seasonService.CreateSeasonAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SeasonRequest request)
        {
            var result = await _seasonService.UpdateSeasonAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _seasonService.DeleteSeasonAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
