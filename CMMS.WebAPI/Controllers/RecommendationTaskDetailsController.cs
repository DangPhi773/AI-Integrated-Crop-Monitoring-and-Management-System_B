using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationTaskDetailsController : ControllerBase
    {
        private readonly IRecommendationTaskDetailService _service;
        public RecommendationTaskDetailsController(IRecommendationTaskDetailService service) => _service = service;

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskId(Guid taskId) => Ok(await _service.GetByTaskIdAsync(taskId));
        
        [Authorize(Roles = "Owner,Worker")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));
        
        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecommendationTaskDetailRequest request) => Ok(await _service.CreateAsync(request));

        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecommendationTaskDetailRequest request) => Ok(await _service.UpdateAsync(id, request));

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
    }
}
