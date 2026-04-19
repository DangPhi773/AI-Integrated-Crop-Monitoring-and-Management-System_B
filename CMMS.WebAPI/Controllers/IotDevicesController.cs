using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.IotDevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IotDevicesController : ControllerBase
    {
        private readonly IIotDeviceService _service;

        public IotDevicesController(IIotDeviceService service) => _service = service;

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllDevicesAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetDeviceByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IotDeviceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _service.CreateDeviceAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] IotDeviceRequest request)
        {
            var result = await _service.UpdateDeviceAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteDeviceAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("{id:guid}/rotate-key")]
        public async Task<IActionResult> RotateKey(Guid id)
        {
            var result = await _service.RegenerateApiKeyAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
