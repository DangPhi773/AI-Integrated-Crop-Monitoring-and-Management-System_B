using CMMS.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlantNetController : ControllerBase
    {
        private readonly IPlantNetService _plantNetService;

        public PlantNetController(IPlantNetService plantNetService)
        {
            _plantNetService = plantNetService;
        }

        [HttpPost("identify")]
        public async Task<IActionResult> Identify(IFormFile image, [FromForm] string organ = "auto")
        {
            if (image == null || image.Length == 0)
                return BadRequest(new { success = false, message = "Ảnh không hợp lệ" });

            var result = await _plantNetService.IdentifyDiseaseAsync(image, organ);
            return Ok(new { success = true, data = result });
        }
    }
}
