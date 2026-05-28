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
                    severity = TranslateSeverity(result.Severity),
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

        private string TranslateDisease(string? disease)
        {
            if (string.IsNullOrWhiteSpace(disease))
                return "Chưa xác định rõ";

            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Bacterial Soft Rot", "Thối nhũn do vi khuẩn" },
                { "Soft Rot", "Thối nhũn" },
                { "Black Rot", "Thối đen" },
                { "Head Rot", "Thối bắp" },
                { "Downy Mildew", "Sương mai" },
                { "Powdery Mildew", "Phấn trắng" },
                { "Leaf Spot", "Đốm lá" },
                { "Alternaria Leaf Spot", "Đốm lá Alternaria" },
                { "Root Rot", "Thối rễ" },
                { "Fusarium Wilt", "Héo rũ Fusarium" },
                { "Anthracnose", "Bệnh thán thư" },
                { "Early Blight", "Cháy lá sớm" },
                { "Late Blight", "Cháy lá muộn" },
                { "Unclear", "Chưa xác định rõ" }
            };

            return map.TryGetValue(disease.Trim(), out var translated)
                ? translated
                : TranslateText(disease);
        }

        private string TranslateSeverity(string? severity)
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

        private string TranslateText(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var translations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Unclear", "Chưa xác định rõ" },

                { "Gemini returned invalid JSON. Please try again with a clearer image.", "Gemini trả về dữ liệu không hợp lệ. Vui lòng thử lại với ảnh rõ hơn." },
                { "Upload a clearer plant image", "Tải lên ảnh cây rõ hơn" },
                { "Take photo in good lighting", "Chụp ảnh ở nơi có ánh sáng tốt" },
                { "Avoid blurry or cropped leaves", "Tránh ảnh bị mờ hoặc lá bị cắt mất" },

                { "Brown lesions", "Vết bệnh màu nâu" },
                { "Soft decay", "Mô mềm bị thối" },
                { "Water-soaked tissue", "Mô cây bị úng nước" },
                { "Water-soaked appearance", "Biểu hiện úng nước" },
                { "Yellow leaves", "Lá vàng" },
                { "Brown spots", "Đốm nâu" },
                { "Dark spots", "Đốm đen" },
                { "Leaf spots", "Đốm lá" },
                { "Chewing damage", "Dấu hiệu bị cắn phá" },
                { "Significant chewing damage", "Dấu hiệu bị cắn phá nghiêm trọng" },
                { "Leaf holes", "Lá bị thủng" },
                { "Holes in leaves", "Lá có lỗ thủng" },
                { "Wilting leaves", "Lá bị héo" },
                { "Rotten tissue", "Mô cây bị thối" },

                { "Dark spots on leaves", "Lá xuất hiện đốm đen" },
                { "Dark spots on inner leaves", "Lá bên trong xuất hiện đốm đen" },
                { "Brown soft decay on cabbage head", "Bắp cải bị thối mềm màu nâu" },
                { "Brown, soft, watery decay on the cabbage head.", "Phần bắp cải bị thối mềm, màu nâu và có dấu hiệu úng nước." },
                { "No clear disease symptoms detected.", "Chưa phát hiện triệu chứng bệnh rõ ràng." },

                { "Improve air circulation", "Tăng độ thông thoáng không khí" },
                { "Avoid overhead irrigation", "Tránh tưới nước trực tiếp lên lá" },
                { "Ensure good drainage", "Đảm bảo đất thoát nước tốt" },
                { "Remove infected plants", "Loại bỏ cây bị nhiễm bệnh" },
                { "Remove and destroy infected plants", "Loại bỏ và tiêu hủy cây bị bệnh" },
                { "Sanitize tools", "Vệ sinh dụng cụ sau khi xử lý" },
                { "Practice crop rotation", "Luân canh cây trồng" },
                { "Monitor the plant", "Theo dõi tình trạng cây" },
                { "Monitor the plant for 2 days", "Theo dõi cây trong 2 ngày" },
                { "Avoid watering leaves", "Tránh tưới nước lên lá" },

                { "Remove infected tissue", "Loại bỏ phần cây bị bệnh" },
                { "Use copper-based bactericide", "Sử dụng thuốc diệt khuẩn gốc đồng" },
                { "Use copper-based bactericide if appropriate", "Sử dụng thuốc diệt khuẩn gốc đồng nếu phù hợp" },
                { "Apply copper-based bactericides", "Sử dụng thuốc diệt khuẩn gốc đồng theo hướng dẫn" },
                { "Apply fungicide", "Sử dụng thuốc nấm phù hợp" },
                { "Take a clearer close-up photo if symptoms spread", "Chụp ảnh cận cảnh rõ hơn nếu triệu chứng lan rộng" }
            };

            var result = text.Trim();

            foreach (var item in translations)
            {
                result = result.Replace(item.Key, item.Value, StringComparison.OrdinalIgnoreCase);
            }

            return result;
        }
    }
}