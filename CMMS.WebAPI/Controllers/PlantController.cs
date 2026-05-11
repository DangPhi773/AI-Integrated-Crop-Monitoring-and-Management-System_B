using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "StaffOnly")]
    public class PlantController : ControllerBase
    {
        private readonly IPlantAnalysisService _service;

        public PlantController(IPlantAnalysisService service)
        {
            _service = service;
        }

        [HttpPost("analyze")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10 * 1024 * 1024)]
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

                var context = new PlantAnalysisContextDto
                {
                    FarmId = request.FarmId,
                    PlotId = request.PlotId,
                    BedId = request.BedId,
                    PlantName = request.PlantName,
                    GrowthStage = request.GrowthStage,
                    Temperature = request.Temperature,
                    AirHumidity = request.AirHumidity,
                    SoilMoisture = request.SoilMoisture,
                    LightIntensity = request.LightIntensity,
                    WeatherCondition = request.WeatherCondition
                };

                var result = await _service.AnalyzePlantImageAsync(request.Image, context);

                return Ok(new
                {
                    disease = result.PossibleDisease,
                    confidence = result.Confidence,
                    severity = result.Severity,
                    description = result.Description,
                    symptoms = result.SymptomsDetected,
                    solutions = result.CareSuggestions,
                    treatmentSteps = result.TreatmentSteps,
                    contextUsed = new
                    {
                        farmId = request.FarmId,
                        plotId = request.PlotId,
                        bedId = request.BedId,
                        plantName = request.PlantName,
                        growthStage = request.GrowthStage,
                        temperature = request.Temperature,
                        airHumidity = request.AirHumidity,
                        soilMoisture = request.SoilMoisture,
                        lightIntensity = request.LightIntensity,
                        weatherCondition = request.WeatherCondition
                    }
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