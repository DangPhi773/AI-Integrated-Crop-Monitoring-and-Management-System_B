using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTasksController : ControllerBase
    {
        private readonly ISubTaskService _subTaskService;
        public SubTasksController(ISubTaskService subTaskService) => _subTaskService = subTaskService;

        [HttpGet("task-detail/{taskDetailId}")]
        public async Task<IActionResult> GetByTask(Guid taskDetailId) => Ok(await _subTaskService.GetSubTasksByTaskAsync(taskDetailId));

        [HttpPost]
        public async Task<IActionResult> Create(SubTaskCreateRequest request) => Ok(await _subTaskService.CreateSubTaskAsync(request));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SubTaskUpdateRequest request) => Ok(await _subTaskService.UpdateSubTaskAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _subTaskService.DeleteSubTaskAsync(id));
    }
}
