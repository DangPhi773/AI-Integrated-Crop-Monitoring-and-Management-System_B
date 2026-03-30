using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize (Roles = "Owner")]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkersController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkers()
        {
            var result = await _workerService.GetListOfWorkersAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkerById(Guid id)
        {
            var result = await _workerService.GetWorkerDetailAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateWorker([FromBody] WorkerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Dữ liệu không hợp lệ");

            var result = await _workerService.CreateNewWorkerAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorker(Guid id, [FromBody] WorkerRequest request)
        {
            var result = await _workerService.UpdateWorkerInfoAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Thay đổi trạng thái hoạt động (Active/Inactive) của Worker
        /// </summary>
        /// <param name="id">ID nhân viên</param>
        /// <param name="status">Trạng thái mới</param>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] string status)
        {
            var result = await _workerService.ChangeWorkerStatusAsync(id, status);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(Guid id)
        {
            var result = await _workerService.RemoveWorkerAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}