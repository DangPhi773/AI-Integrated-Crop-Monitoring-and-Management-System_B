using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationTasksController : ControllerBase
    {
        private readonly IRecommendationTaskService _service;
        public RecommendationTasksController(IRecommendationTaskService service) => _service = service;

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [Authorize(Roles = "Owner,Specialist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

        [Authorize(Roles = "Owner,Specialist")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecommendationTaskRequest request) => Ok(await _service.CreateAsync(request));

        [Authorize(Roles = "Owner,Specialist")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecommendationTaskRequest request) => Ok(await _service.UpdateAsync(id, request));
        
        [Authorize(Roles = "Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
    }
}
