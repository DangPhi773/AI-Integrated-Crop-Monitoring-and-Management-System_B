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

        public async Task<PlantAnalysisResultDto> AnalyzePlantImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new Exception("Thiếu ảnh.");

            if (!image.ContentType.StartsWith("image/"))
                throw new Exception("File phải là ảnh.");

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

            var endpoint = $"{baseUrl}/models/{model}:generateContent?key={apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = GetPrompt() },
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
                    responseMimeType = "application/json"
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Gemini lỗi: {responseText}");

            var root = JObject.Parse(responseText);
            var modelText = root["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

            if (string.IsNullOrWhiteSpace(modelText))
                throw new Exception("Không có dữ liệu trả về từ Gemini.");

            var result = JsonConvert.DeserializeObject<PlantAnalysisResultDto>(modelText);

            if (result == null)
                throw new Exception("Parse JSON thất bại.");

            return result;
        }

        private string GetPrompt()
        {
            return """
Bạn là trợ lý hỗ trợ nhận diện bệnh cây từ ảnh.

Yêu cầu:
- Phân tích ảnh cây hoặc lá cây.
- Không khẳng định tuyệt đối.
- Nếu không chắc chắn, ghi "Chưa xác định rõ".
- Trả về JSON hợp lệ.

Schema:
{
  "possibleDisease": string,
  "confidence": number,
  "description": string,
  "symptomsDetected": [string],
  "careSuggestions": [string],
  "severity": "low | medium | high"
}

Chỉ trả JSON.
""";
        }
    }
}