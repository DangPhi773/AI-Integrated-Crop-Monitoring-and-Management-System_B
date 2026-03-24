using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskDetailsController : ControllerBase
    {
        private readonly ITaskDetailService _service;

        public TaskDetailsController(ITaskDetailService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("season/{seasonId:guid}")]
        public async Task<IActionResult> GetBySeason(Guid seasonId)
        {
            var result = await _service.GetBySeasonIdAsync(seasonId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("worker/{workerId:guid}")]
        public async Task<IActionResult> GetByWorker(Guid workerId)
        {
            var result = await _service.GetByWorkerIdAsync(workerId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("bed/{bedId:guid}")]
        public async Task<IActionResult> GetByBed(Guid bedId)
        {
            var result = await _service.GetByBedIdAsync(bedId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("task/{taskId:guid}")]
        public async Task<IActionResult> GetByTask(Guid taskId)
        {
            var result = await _service.GetByTaskIdAsync(taskId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskDetailRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _service.CreateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskDetailRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
