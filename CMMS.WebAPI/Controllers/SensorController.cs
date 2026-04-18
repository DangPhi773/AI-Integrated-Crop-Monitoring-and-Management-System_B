using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.IotDatas;
using CMMS.DAL.Entities;
using CMMS.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/sensors")]
    public class SensorController : ControllerBase
    {
        private readonly ISensorDataService _service;

        public SensorController(ISensorDataService service) => _service = service;

        [HttpPost]
        [DeviceApiKey]
        public async Task<IActionResult> ReceiveSensorData([FromBody] SensorDataRequest request)
        {
            var device = HttpContext.Items[DeviceApiKeyAttribute.ContextKey] as IotDevice;
            if (device == null)
                return Unauthorized(new { message = "Device chưa xác thực." });

            var result = await _service.ProcessSensorDataAsync(device, request);
            return StatusCode(201, result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest([FromQuery] string deviceCode)
        {
            var result = await _service.GetLatestAsync(deviceCode);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] string deviceCode, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _service.GetHistoryAsync(deviceCode, from, to);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
