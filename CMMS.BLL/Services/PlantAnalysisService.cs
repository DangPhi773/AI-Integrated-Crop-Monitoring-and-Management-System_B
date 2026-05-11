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
                    maxOutputTokens = 1000,
                    temperature = 0.1
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
                result = ParseGeminiResult(modelText);
            }
            catch (Exception ex)
            {
                throw new Exception($"Parse Gemini JSON lỗi. Raw: {modelText}. Error: {ex.Message}");
            }

            NormalizeResult(result);

            return result;
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
                return token.Select(x => x?.ToString() ?? string.Empty)
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .ToList();

            var text = token.ToString();

            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            return new List<string> { text };
        }

        private void NormalizeResult(PlantAnalysisResultDto result)
        {
            result.PossibleDisease ??= string.Empty;
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
Return only a valid compact JSON object.
Do not use markdown.
Do not add explanation outside JSON.
Use English only.
Use short string values.
Do not break strings with newlines.
If uncertain, set possibleDisease to ""Unclear"".
confidence must be 0 to 1.
severity must be one of: low, medium, high.

JSON example:
{{""possibleDisease"":""Bacterial Soft Rot"",""confidence"":0.85,""description"":""Brown soft decay on cabbage head."",""symptomsDetected"":[""Brown lesions"",""Soft decay"",""Water-soaked tissue""],""careSuggestions"":[""Improve air circulation"",""Avoid overhead irrigation""],""treatmentSteps"":[""Remove infected tissue"",""Use copper-based bactericide if appropriate""],""severity"":""high""}}";
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