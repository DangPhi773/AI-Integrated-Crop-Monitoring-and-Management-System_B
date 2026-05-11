using System.Text;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMS.BLL.Services
{
    public class PlantAnalysisService : IPlantAnalysisService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PlantAnalysisService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        private static readonly string[] AllowedMimeTypes =
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "image/heic",
            "image/heif"
        };

        private const long MaxImageBytes = 2 * 1024 * 1024;

        public async Task<PlantAnalysisResultDto> AnalyzePlantImageAsync(
            IFormFile image,
            PlantAnalysisContextDto context)
        {
            if (image == null || image.Length == 0)
                throw new Exception("Thiếu ảnh.");

            if (image.Length > MaxImageBytes)
                throw new Exception("Ảnh quá lớn. Vui lòng nén ảnh dưới 2MB trước khi gửi.");

            if (string.IsNullOrWhiteSpace(image.ContentType) ||
                !AllowedMimeTypes.Contains(image.ContentType.ToLowerInvariant()))
                throw new Exception("Định dạng ảnh không hỗ trợ. Chỉ chấp nhận JPEG, PNG, WEBP, HEIC/HEIF.");

            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            var base64Image = Convert.ToBase64String(imageBytes);

            var apiKey = _configuration["Gemini:ApiKey"];
            var model = _configuration["Gemini:Model"];
            var baseUrl = _configuration["Gemini:BaseUrl"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("Thiếu Gemini API key.");

            if (string.IsNullOrWhiteSpace(model))
                throw new Exception("Thiếu Gemini model.");

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new Exception("Thiếu Gemini base url.");

            var endpoint = $"{baseUrl}/models/{model}:generateContent";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = GetPrompt(context) },
                            new
                            {
                                inline_data = new
                                {
                                    mime_type = image.ContentType,
                                    data = base64Image
                                }
                            }
                        }
                    }
                },
                generationConfig = new
                {
                    responseMimeType = "application/json",
                    maxOutputTokens = 500,
                    temperature = 0.2
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("x-goog-api-key", apiKey);

            var response = await _httpClient.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Gemini lỗi: {responseText}");

            var root = JObject.Parse(responseText);
            var modelText = root["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

            if (string.IsNullOrWhiteSpace(modelText))
                throw new Exception("Không có dữ liệu trả về từ Gemini.");

            PlantAnalysisResultDto result;

            try
            {
                modelText = CleanJson(modelText);
                result = JsonConvert.DeserializeObject<PlantAnalysisResultDto>(modelText)
                         ?? CreateFallbackResult();
            }
            catch
            {
                result = CreateFallbackResult();
            }

            NormalizeResult(result);

            return result;
        }

        private PlantAnalysisResultDto CreateFallbackResult()
        {
            return new PlantAnalysisResultDto
            {
                PossibleDisease = "Chưa xác định rõ",
                Confidence = 0,
                Description = "AI chưa trả về kết quả đúng định dạng. Vui lòng thử lại với ảnh rõ hơn.",
                SymptomsDetected = new List<string>
                {
                    "Không xác định rõ triệu chứng từ ảnh hiện tại"
                },
                CareSuggestions = new List<string>
                {
                    "Chụp lại ảnh cây rõ hơn, đủ sáng và tập trung vào lá/cành có dấu hiệu bệnh."
                },
                TreatmentSteps = new List<string>
                {
                    "Theo dõi cây thêm 24-48 giờ.",
                    "Kiểm tra thủ công các lá bị vàng, đốm hoặc héo.",
                    "Nếu triệu chứng lan rộng, gửi báo cáo cho owner hoặc chuyên gia."
                },
                Severity = "low"
            };
        }

        private void NormalizeResult(PlantAnalysisResultDto result)
        {
            result.PossibleDisease ??= string.Empty;
            result.Description ??= string.Empty;
            result.SymptomsDetected ??= new List<string>();
            result.CareSuggestions ??= new List<string>();
            result.TreatmentSteps ??= new List<string>();
            result.Severity = NormalizeSeverity(result.Severity);
        }

        private string NormalizeSeverity(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "low";

            var level = value.Trim().ToLowerInvariant();

            return level switch
            {
                "low" => "low",
                "medium" => "medium",
                "high" => "high",
                _ => "low"
            };
        }

        private string GetPrompt(PlantAnalysisContextDto context)
        {
            return $@"Analyze the plant image and environment data.

Context:
Plant={ShortText(context.PlantName, 40)}
Stage={ShortText(context.GrowthStage, 40)}
Temp={FormatNumber(context.Temperature)}C
AirHumidity={FormatNumber(context.AirHumidity)}%
SoilMoisture={FormatNumber(context.SoilMoisture)}%
Light={FormatNumber(context.LightIntensity)}
Weather={ShortText(context.WeatherCondition, 40)}

Return ONLY valid minified JSON.
Do not use markdown.
Do not add explanation.
All string values must be in English.
If unsure, use ""Unclear"".
Symptoms and treatment must be specific.

Example:
{{""possibleDisease"":""Unclear"",""confidence"":0.5,""description"":""No clear disease symptoms detected."",""symptomsDetected"":[""No clear symptoms""],""careSuggestions"":[""Monitor the plant for 2 days""],""treatmentSteps"":[""Take a clearer close-up photo if symptoms spread""],""severity"":""low""}}";
        }

        private string CleanJson(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = text.Trim();

            text = text.Replace("```json", "");
            text = text.Replace("```", "");

            var start = text.IndexOf('{');
            var end = text.LastIndexOf('}');

            if (start >= 0 && end > start)
                text = text.Substring(start, end - start + 1);

            return text.Trim();
        }

        private string ShortText(string? value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "N/A";

            value = value.Trim();

            return value.Length <= maxLength
                ? value
                : value.Substring(0, maxLength);
        }

        private string FormatNumber(double? value)
        {
            return value.HasValue ? value.Value.ToString("0.#") : "N/A";
        }
    }
}