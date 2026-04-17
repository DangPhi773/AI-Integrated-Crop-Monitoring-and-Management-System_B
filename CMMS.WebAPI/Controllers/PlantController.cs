using CMMS.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly IPlantAnalysisService _service;

        public PlantController(IPlantAnalysisService service)
        {
            _service = service;
        }

        [HttpPost("analyze")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Analyze([FromForm] PlantImageUploadRequestDto request)
        {
            try
            {
                if (request.Image == null)
                {
                    return BadRequest(new
                    {
                        message = "Thiếu ảnh"
                    });
                }

                var result = await _service.AnalyzePlantImageAsync(request.Image);

                return Ok(new
                {
                    disease = result.PossibleDisease,
                    confidence = result.Confidence,
                    severity = result.Severity,
                    description = result.Description,
                    symptoms = result.SymptomsDetected,
                    solutions = result.CareSuggestions
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}