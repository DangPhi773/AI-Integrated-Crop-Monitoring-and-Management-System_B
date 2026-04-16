using System.Text;
using System.Text.Json;
using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CMMS.BLL.Services;

public class GeminiDiseaseService : IGeminiDiseaseService
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;

    public GeminiDiseaseService(HttpClient httpClient, IOptions<GeminiSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task<DiseaseAnalysisResponse> AnalyzeImageAsync(IFormFile image, string? organ = null)
    {
        var base64 = await ConvertToBase64(image);
        var prompt = BuildPrompt(organ);

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = prompt },
                        new { inline_data = new { mime_type = image.ContentType, data = base64 } }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 0.2,
                maxOutputTokens = 2048
            }
        };

        var url = $"{_settings.BaseUrl}/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return new DiseaseAnalysisResponse
            {
                Results = new List<DiseaseResult>(),
                RawResponse = json
            };
        }

        return ParseResponse(json);
    }

    private static string BuildPrompt(string? organ)
    {
        var organHint = string.IsNullOrEmpty(organ) ? "" : $" Bộ phận cây trong ảnh: {organ}.";
        return $@"Bạn là chuyên gia bệnh học thực vật. Phân tích ảnh cây trồng này và xác định bệnh (nếu có).{organHint}

Trả về KẾT QUẢ DUY NHẤT là JSON array (không markdown, không giải thích thêm) với format:
[{{
  ""name"": ""tên bệnh (khoa học/tiếng Anh)"",
  ""label"": ""tên thường gọi (tiếng Việt)"",
  ""score"": 0.85,
  ""severity"": ""HIGH"",
  ""categories"": [""fungal""],
  ""recommendation"": ""Khuyến nghị xử lý ngắn gọn bằng tiếng Việt""
}}]

Quy tắc:
- score: 0.0 đến 1.0, thể hiện độ tin cậy
- severity: HIGH (>=0.7), MEDIUM (>=0.4), LOW (<0.4)
- categories: fungal, bacterial, viral, nutrient_deficiency, pest, environmental
- Nếu cây khỏe mạnh, trả về: []
- Nếu phát hiện nhiều bệnh, trả về nhiều object trong array
- Chỉ trả về JSON, không có text nào khác";
    }

    private static DiseaseAnalysisResponse ParseResponse(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var text = root
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "[]";

            text = text.Trim();
            if (text.StartsWith("```"))
            {
                var startIdx = text.IndexOf('[');
                var endIdx = text.LastIndexOf(']');
                if (startIdx >= 0 && endIdx >= 0)
                    text = text[startIdx..(endIdx + 1)];
            }

            var results = JsonSerializer.Deserialize<List<DiseaseResult>>(text, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<DiseaseResult>();

            return new DiseaseAnalysisResponse
            {
                Results = results,
                RawResponse = null
            };
        }
        catch (Exception ex)
        {
            return new DiseaseAnalysisResponse
            {
                Results = new List<DiseaseResult>(),
                RawResponse = $"Parse error: {ex.Message}"
            };
        }
    }

    private static async Task<string> ConvertToBase64(IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return Convert.ToBase64String(ms.ToArray());
    }
}
