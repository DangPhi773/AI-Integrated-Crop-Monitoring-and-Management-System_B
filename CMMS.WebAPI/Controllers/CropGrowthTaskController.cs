using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CropGrowthTaskController : ControllerBase
    {
        private readonly ICropGrowthTaskService _service;
        public CropGrowthTaskController(ICropGrowthTaskService service) => _service = service;

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [Authorize(Roles = "Owner")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _service.GetByIdAsync(id);
            return res.Success ? Ok(res) : NotFound(res);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("stage/{stageId}")]
        public async Task<IActionResult> GetByStage(Guid stageId) => Ok(await _service.GetByStageIdAsync(stageId));

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create(CropGrowthTaskRequest request)
        {
            var res = await _service.CreateAsync(request);
            return res.Success ? StatusCode(201, res) : BadRequest(res);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CropGrowthTaskRequest request) => Ok(await _service.UpdateAsync(id, request));

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
    }
}
