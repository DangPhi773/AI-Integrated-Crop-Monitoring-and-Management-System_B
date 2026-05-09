using CMMS.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "StaffOnly")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _service;

        public WeatherController(IWeatherService service)
        {
            _service = service;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent([FromQuery] decimal lat, [FromQuery] decimal lng)
        {
            try
            {
                var result = await _service.GetCurrentAsync(lat, lng);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecast([FromQuery] decimal lat, [FromQuery] decimal lng, [FromQuery] int days = 3)
        {
            try
            {
                var result = await _service.GetForecastAsync(lat, lng, days);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("farm/{farmId:guid}/current")]
        public async Task<IActionResult> GetCurrentByFarm(Guid farmId)
        {
            try
            {
                var result = await _service.GetCurrentByFarmAsync(farmId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("farm/{farmId:guid}/forecast")]
        public async Task<IActionResult> GetForecastByFarm(Guid farmId, [FromQuery] int days = 3)
        {
            try
            {
                var result = await _service.GetForecastByFarmAsync(farmId, days);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
