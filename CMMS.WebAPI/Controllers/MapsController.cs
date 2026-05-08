using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Maps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "StaffOnly")]
    public class MapsController : ControllerBase
    {
        private readonly IGoogleMapsService _service;

        public MapsController(IGoogleMapsService service)
        {
            _service = service;
        }

        [HttpGet("geocode")]
        public async Task<IActionResult> Geocode([FromQuery] string address)
        {
            try
            {
                var result = await _service.GeocodeAsync(address);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("reverse")]
        public async Task<IActionResult> ReverseGeocode([FromQuery] decimal lat, [FromQuery] decimal lng)
        {
            try
            {
                var result = await _service.ReverseGeocodeAsync(lat, lng);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("farm/{farmId:guid}/coordinates")]
        public async Task<IActionResult> UpdateFarmCoordinates(Guid farmId, [FromBody] UpdateFarmCoordinatesRequestDto request)
        {
            try
            {
                var result = await _service.UpdateFarmCoordinatesAsync(farmId, request);
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
