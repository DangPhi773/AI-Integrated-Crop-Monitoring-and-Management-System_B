using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.IotDevices;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IotDevicesController : ControllerBase
    {
        private readonly IIotDeviceService _service;

        public IotDevicesController(IIotDeviceService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllDevicesAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetDeviceByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IotDeviceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _service.CreateDeviceAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] IotDeviceRequest request)
        {
            var result = await _service.UpdateDeviceAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteDeviceAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
