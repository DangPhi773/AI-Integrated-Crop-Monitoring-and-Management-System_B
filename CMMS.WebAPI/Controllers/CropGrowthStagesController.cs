using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Crops;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize(Roles = "Owner,Specialist")]
    [Route("api/[controller]")]
    [ApiController]
    public class CropGrowthStagesController : ControllerBase
    {
        private readonly ICropGrowthStageService _service;
        public CropGrowthStagesController(ICropGrowthStageService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetStagesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetStageByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CropGrowthStageRequest request) => Ok(await _service.CreateStageAsync(request));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CropGrowthStageRequest request) => Ok(await _service.UpdateStageAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _service.RemoveStageAsync(id));
    }
}
