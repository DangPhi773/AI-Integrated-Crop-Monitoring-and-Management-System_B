using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.IotDatas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize(Roles = "Owner,Worker")]
    [ApiController]
    [Route("api/[controller]")]
    public class IotDatasController : ControllerBase
    {
        private readonly IIotDataService _service;

        public IotDatasController(IIotDataService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllDataAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetDataByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("device/{deviceId:guid}")]
        public async Task<IActionResult> GetByDeviceId(Guid deviceId)
        {
            var result = await _service.GetDataByDeviceIdAsync(deviceId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IotDataRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _service.CreateDataAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteDataAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
