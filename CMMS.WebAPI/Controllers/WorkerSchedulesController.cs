using CMMS.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerSchedulesController : ControllerBase
    {
        private readonly IWorkerScheduleService _service;

        public WorkerSchedulesController(IWorkerScheduleService service) => _service = service;

        [Authorize(Roles = "Worker")]
        [HttpGet("my-schedule")]
        public async Task<IActionResult> GetMySchedule()
        {
            var workerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _service.GetMyScheduleAsync(workerId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("worker/{workerId:guid}")]
        public async Task<IActionResult> GetByWorker(Guid workerId)
        {
            var result = await _service.GetByWorkerIdAsync(workerId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("task-detail/{taskDetailId:guid}")]
        public async Task<IActionResult> GetByTaskDetail(Guid taskDetailId)
        {
            var result = await _service.GetByTaskDetailIdAsync(taskDetailId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
