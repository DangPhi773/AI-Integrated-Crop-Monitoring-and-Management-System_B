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
                    disease = TranslateDisease(result.PossibleDisease),
                    confidence = result.Confidence,
                    severity = TranslateSeverity(result.Severity),
                    description = TranslateText(result.Description),
                    symptoms = result.SymptomsDetected.Select(TranslateText).ToList(),
                    solutions = result.CareSuggestions.Select(TranslateText).ToList(),
                    treatmentSteps = result.TreatmentSteps.Select(TranslateText).ToList(),
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

        private string TranslateDisease(string disease)
        {
            if (string.IsNullOrWhiteSpace(disease))
                return "Chưa xác định rõ";

            return disease.Trim() switch
            {
                "Bacterial Soft Rot" => "Thối nhũn do vi khuẩn",
                "Soft Rot" => "Thối nhũn",
                "Black Rot" => "Thối đen",
                "Head Rot" => "Thối bắp",
                "Downy Mildew" => "Sương mai",
                "Powdery Mildew" => "Phấn trắng",
                "Leaf Spot" => "Đốm lá",
                "Root Rot" => "Thối rễ",
                "Fusarium Wilt" => "Héo rũ Fusarium",
                "Anthracnose" => "Thán thư",
                "Early Blight" => "Bệnh cháy lá sớm",
                "Late Blight" => "Bệnh cháy lá muộn",
                "Unclear" => "Chưa xác định rõ",
                _ => disease
            };
        }

        private string TranslateSeverity(string severity)
        {
            if (string.IsNullOrWhiteSpace(severity))
                return "Thấp";

            return severity.Trim().ToLowerInvariant() switch
            {
                "low" => "Thấp",
                "medium" => "Trung bình",
                "high" => "Cao",
                _ => severity
            };
        }

        private string TranslateText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            return text
                .Replace("Cabbage head shows extensive brown, water-soaked, soft decay, primarily affecting the head.",
                    "Phần bắp cải có dấu hiệu thối mềm, màu nâu và úng nước, chủ yếu ảnh hưởng ở phần bắp.")
                .Replace("Brown, soft, watery decay on the cabbage head.",
                    "Phần bắp cải bị thối mềm, màu nâu và có dấu hiệu úng nước.")
                .Replace("Brown soft decay on cabbage head.",
                    "Phần bắp cải có dấu hiệu thối mềm màu nâu.")
                .Replace("No clear disease symptoms detected.",
                    "Chưa phát hiện triệu chứng bệnh rõ ràng.")
                .Replace("Short symptom description.",
                    "Mô tả ngắn triệu chứng quan sát được.")
                .Replace("Brown lesions",
                    "Vết bệnh màu nâu")
                .Replace("Soft decay",
                    "Mô mềm bị thối")
                .Replace("Water-soaked appearance on head",
                    "Phần bắp có biểu hiện úng nước")
                .Replace("Water-soaked appearance",
                    "Biểu hiện úng nước")
                .Replace("Water-soaked tissue",
                    "Mô cây bị úng nước")
                .Replace("Yellow leaves",
                    "Lá vàng")
                .Replace("Brown spots",
                    "Đốm nâu")
                .Replace("No clear symptoms",
                    "Chưa có triệu chứng rõ ràng")
                .Replace("Improve air circulation",
                    "Tăng độ thông thoáng không khí")
                .Replace("Avoid overhead irrigation",
                    "Tránh tưới nước trực tiếp lên lá hoặc phần bắp")
                .Replace("Ensure good drainage",
                    "Đảm bảo đất thoát nước tốt")
                .Replace("Remove infected plants",
                    "Loại bỏ cây bị nhiễm bệnh")
                .Replace("Remove and destroy infected plants",
                    "Loại bỏ và tiêu hủy cây bị bệnh")
                .Replace("Remove infected tissue",
                    "Loại bỏ phần mô bị nhiễm bệnh")
                .Replace("Apply copper-based bactericides",
                    "Sử dụng thuốc diệt khuẩn gốc đồng theo hướng dẫn")
                .Replace("Use copper-based bactericide if appropriate",
                    "Sử dụng thuốc diệt khuẩn gốc đồng nếu phù hợp")
                .Replace("Sanitize tools",
                    "Vệ sinh dụng cụ sau khi xử lý cây bệnh")
                .Replace("Practice crop rotation",
                    "Luân canh cây trồng để hạn chế mầm bệnh")
                .Replace("Monitor the plant for 2 days",
                    "Theo dõi cây trong 2 ngày")
                .Replace("Take a clearer close-up photo if symptoms spread",
                    "Chụp ảnh cận cảnh rõ hơn nếu triệu chứng lan rộng")
                .Replace("Avoid watering leaves",
                    "Tránh tưới nước lên lá")
                .Replace("Unclear",
                    "Chưa xác định rõ");
        }
    }
}