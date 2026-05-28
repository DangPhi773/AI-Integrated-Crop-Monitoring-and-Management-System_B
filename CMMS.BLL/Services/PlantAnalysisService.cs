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
                    maxOutputTokens = 1500,
                    temperature = 0
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

            var modelText = ExtractGeminiText(responseText);

            if (string.IsNullOrWhiteSpace(modelText))
                throw new Exception("Không có dữ liệu trả về từ Gemini.");

            try
            {
                modelText = CleanJson(modelText);
                var result = ParseGeminiResult(modelText);
                NormalizeResult(result);
                return result;
            }
            catch
            {
                return CreateFallbackResult();
            }
        }

        private string? ExtractGeminiText(string responseText)
        {
            var root = JObject.Parse(responseText);

            return root["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
        }

        private PlantAnalysisResultDto ParseGeminiResult(string json)
        {
            var obj = JObject.Parse(json);

            return new PlantAnalysisResultDto
            {
                PossibleDisease = obj["possibleDisease"]?.ToString() ?? "Unclear",
                Confidence = obj["confidence"]?.ToObject<double?>() ?? 0,
                Description = obj["description"]?.ToString() ?? string.Empty,
                SymptomsDetected = ToStringList(obj["symptomsDetected"]),
                CareSuggestions = ToStringList(obj["careSuggestions"]),
                TreatmentSteps = ToStringList(obj["treatmentSteps"]),
                Severity = obj["severity"]?.ToString() ?? "low"
            };
        }

        private List<string> ToStringList(JToken? token)
        {
            if (token == null)
                return new List<string>();

            if (token.Type == JTokenType.Array)
            {
                return token.Select(x => x?.ToString() ?? string.Empty)
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .ToList();
            }

            var text = token.ToString();

            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            return new List<string> { text };
        }

        private void NormalizeResult(PlantAnalysisResultDto result)
        {
            result.PossibleDisease ??= "Unclear";
            result.Description ??= string.Empty;
            result.SymptomsDetected ??= new List<string>();
            result.CareSuggestions ??= new List<string>();
            result.TreatmentSteps ??= new List<string>();
            result.Severity = NormalizeSeverity(result.Severity);

            if (result.Confidence < 0)
                result.Confidence = 0;

            if (result.Confidence > 1)
                result.Confidence = 1;
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

        private PlantAnalysisResultDto CreateFallbackResult()
        {
            return new PlantAnalysisResultDto
            {
                PossibleDisease = "Unclear",
                Confidence = 0,
                Description = "Gemini returned invalid JSON. Please try again with a clearer image.",
                SymptomsDetected = new List<string>(),
                CareSuggestions = new List<string>
                {
                    "Upload a clearer plant image",
                    "Take photo in good lighting",
                    "Avoid blurry or cropped leaves"
                },
                TreatmentSteps = new List<string>(),
                Severity = "low"
            };
        }

        private string GetPrompt(PlantAnalysisContextDto context)
        {
            return $@"Analyze plant disease from image and environment data.

            Context:
            Plant={ShortText(context.PlantName, 40)}
            Stage={ShortText(context.GrowthStage, 40)}
            Temp={FormatNumber(context.Temperature)}C
            AirHumidity={FormatNumber(context.AirHumidity)}%
            SoilMoisture={FormatNumber(context.SoilMoisture)}%
            Light={FormatNumber(context.LightIntensity)}
            Weather={ShortText(context.WeatherCondition, 40)}

            Task:
            Identify the most likely plant disease, visible symptoms, and practical treatment.

            Rules:
            Return only one valid compact JSON object.
            Do not use markdown.
            Do not add explanation outside JSON.
            JSON keys must stay in English.
            All JSON values must be in Vietnamese.
            Use natural Vietnamese for farmers.
            Use short string values.
            Do not break strings with newlines.
            All string values must be complete and closed.
            Do not use trailing commas.
            description max 120 characters.
            Each array item max 60 characters.
            If uncertain, set possibleDisease to ""Chưa xác định rõ"".
            confidence must be 0 to 1.
            severity must be one of: low, medium, high .

            Required JSON format:
            {{""possibleDisease"":""Sâu ăn lá bắp cải"",""confidence"":0.9,""description"":""Lá bị sâu cắn tạo nhiều lỗ thủng."",""symptomsDetected"":[""Lá có lỗ thủng"",""Mép lá bị cắn phá""],""careSuggestions"":[""Kiểm tra cây thường xuyên"",""Loại bỏ lá bị hại""],""treatmentSteps"":[""Bắt sâu bằng tay"",""Dùng chế phẩm sinh học phù hợp""],""severity"":""medium""}}";
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