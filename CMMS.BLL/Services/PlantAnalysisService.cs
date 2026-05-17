using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CMMS.BLL.Services
{
    public class PlantAnalysisService : IPlantAnalysisService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private static readonly string[] AllowedMimeTypes =
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "image/heic",
            "image/heif"
        };

        private const long MaxImageBytes = 10 * 1024 * 1024;
        private const string ModelVersion = "gemini-vision-n8n-v1";

        public PlantAnalysisService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            var timeoutSeconds = _configuration.GetValue("N8nAi:TimeoutSeconds", 60);
            _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        }

        public async Task<PlantAnalysisResultDto> AnalyzePlantImageAsync(
            IFormFile image,
            PlantAnalysisContextDto context)
        {
            ValidateImage(image);

            var webhookUrl = _configuration["N8nAi:WebhookUrl"];
            if (string.IsNullOrWhiteSpace(webhookUrl))
                throw new Exception("Thiếu cấu hình N8nAi:WebhookUrl.");

            var timeoutSeconds = _configuration.GetValue("N8nAi:TimeoutSeconds", 60);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));

            using var form = new MultipartFormDataContent();

            await using var imageStream = image.OpenReadStream();
            using var imageContent = new StreamContent(imageStream);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
            form.Add(imageContent, "image", image.FileName);

            AddFormField(form, "farmId", context.FarmId?.ToString());
            AddFormField(form, "plotId", context.PlotId?.ToString());
            AddFormField(form, "bedId", context.BedId?.ToString());
            AddFormField(form, "plantName", context.PlantName);
            AddFormField(form, "growthStage", context.GrowthStage);
            AddFormField(form, "temperature", context.Temperature?.ToString(CultureInfo.InvariantCulture));
            AddFormField(form, "airHumidity", context.AirHumidity?.ToString(CultureInfo.InvariantCulture));
            AddFormField(form, "soilMoisture", context.SoilMoisture?.ToString(CultureInfo.InvariantCulture));
            AddFormField(form, "lightIntensity", context.LightIntensity?.ToString(CultureInfo.InvariantCulture));
            AddFormField(form, "weatherCondition", context.WeatherCondition);
            AddFormField(form, "language", "vi");

            using var request = new HttpRequestMessage(HttpMethod.Post, webhookUrl)
            {
                Content = form
            };

            var secret = _configuration["N8nAi:Secret"];
            if (!string.IsNullOrWhiteSpace(secret))
                request.Headers.Add("X-Crop-AI-Secret", secret);

            using var response = await _httpClient.SendAsync(request, cts.Token);
            var raw = await response.Content.ReadAsStringAsync(cts.Token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"n8n AI workflow lỗi {(int)response.StatusCode}: {raw}");

            var n8nResult = ParseN8nResponse(raw);
            return BuildResult(n8nResult, context);
        }

        private static void AddFormField(MultipartFormDataContent form, string name, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                form.Add(new StringContent(value, Encoding.UTF8), name);
        }

        private static N8nDiagnosisResponse ParseN8nResponse(string raw)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var trimmed = raw.Trim();

                if (trimmed.StartsWith("["))
                {
                    var list = JsonSerializer.Deserialize<List<N8nDiagnosisResponse>>(trimmed, options);
                    if (list is { Count: > 0 })
                        return list[0];
                }

                var result = JsonSerializer.Deserialize<N8nDiagnosisResponse>(trimmed, options);
                if (result == null)
                    throw new Exception("Response rỗng.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Parse n8n/Gemini JSON lỗi. Raw: {raw}. Error: {ex.Message}");
            }
        }

        private static PlantAnalysisResultDto BuildResult(N8nDiagnosisResponse result, PlantAnalysisContextDto context)
        {
            var disease = FirstNonEmpty(result.Disease, result.PossibleDisease, "Không xác định");
            var diseaseCode = FirstNonEmpty(result.DiseaseCode, ToDiseaseCode(disease));
            var confidence = Clamp(result.Confidence ?? 0.5);
            var severity = FirstNonEmpty(result.Severity, "Trung bình");
            var description = FirstNonEmpty(
                result.Description,
                $"Gemini dự đoán cây {context.PlantName} có khả năng mắc: {disease}."
            );

            var symptoms = NormalizeList(
                result.Symptoms,
                "Chưa phát hiện triệu chứng cụ thể từ phản hồi Gemini."
            );

            var solutions = NormalizeList(
                result.Solutions ?? result.CareSuggestions,
                "Theo dõi cây và kiểm tra lại sau 2–3 ngày."
            );

            var treatmentSteps = NormalizeList(
                result.TreatmentSteps,
                "Liên hệ kỹ thuật viên nếu triệu chứng lan rộng."
            );

            var english = result.English ?? new PlantDiagnosisLanguageDto
            {
                Disease = FirstNonEmpty(result.EnglishDisease, disease),
                Description = FirstNonEmpty(result.EnglishDescription, description),
                Severity = severity,
                Symptoms = symptoms,
                CareSuggestions = solutions,
                TreatmentSteps = treatmentSteps
            };

            var vietnamese = result.Vietnamese ?? new PlantDiagnosisLanguageDto
            {
                Disease = disease,
                Description = description,
                Severity = severity,
                Symptoms = symptoms,
                CareSuggestions = solutions,
                TreatmentSteps = treatmentSteps
            };

            return new PlantAnalysisResultDto
            {
                PossibleDisease = disease,
                DiseaseCode = diseaseCode,
                Confidence = confidence,
                Severity = severity,
                Description = description,
                SymptomsDetected = symptoms,
                CareSuggestions = solutions,
                TreatmentSteps = treatmentSteps,
                IsConfident = confidence >= 0.55,
                ModelVersion = ModelVersion,
                TopPredictions = result.TopPredictions ?? new List<PlantPredictionDto>
                {
                    new PlantPredictionDto
                    {
                        Label = diseaseCode,
                        DisplayName = disease,
                        Confidence = confidence
                    }
                },
                English = english,
                Vietnamese = vietnamese,
                ContextUsed = context
            };
        }

        private static void ValidateImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new Exception("Thiếu ảnh.");

            if (image.Length > MaxImageBytes)
                throw new Exception("Ảnh quá lớn. Vui lòng nén ảnh dưới 10MB trước khi gửi.");

            if (string.IsNullOrWhiteSpace(image.ContentType) ||
                !AllowedMimeTypes.Contains(image.ContentType.ToLowerInvariant()))
            {
                throw new Exception("Định dạng ảnh không hỗ trợ. Chỉ chấp nhận JPEG, PNG, WEBP, HEIC/HEIF.");
            }
        }

        private static List<string> NormalizeList(List<string>? values, string fallback)
        {
            var cleaned = values?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToList();

            return cleaned is { Count: > 0 } ? cleaned : new List<string> { fallback };
        }

        private static string FirstNonEmpty(params string?[] values)
        {
            return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim() ?? string.Empty;
        }

        private static double Clamp(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value)) return 0;
            if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
        }

        private static string ToDiseaseCode(string disease)
        {
            if (string.IsNullOrWhiteSpace(disease))
                return "UNKNOWN";

            var normalized = disease
                .Trim()
                .ToUpperInvariant()
                .Replace(" ", "_")
                .Replace("-", "_")
                .Replace("/", "_");

            return new string(normalized.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());
        }

        private sealed class N8nDiagnosisResponse
        {
            [JsonPropertyName("disease")]
            public string? Disease { get; set; }

            [JsonPropertyName("possibleDisease")]
            public string? PossibleDisease { get; set; }

            [JsonPropertyName("diseaseCode")]
            public string? DiseaseCode { get; set; }

            [JsonPropertyName("confidence")]
            public double? Confidence { get; set; }

            [JsonPropertyName("severity")]
            public string? Severity { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("symptoms")]
            public List<string>? Symptoms { get; set; }

            [JsonPropertyName("solutions")]
            public List<string>? Solutions { get; set; }

            [JsonPropertyName("careSuggestions")]
            public List<string>? CareSuggestions { get; set; }

            [JsonPropertyName("treatmentSteps")]
            public List<string>? TreatmentSteps { get; set; }

            [JsonPropertyName("topPredictions")]
            public List<PlantPredictionDto>? TopPredictions { get; set; }

            [JsonPropertyName("english")]
            public PlantDiagnosisLanguageDto? English { get; set; }

            [JsonPropertyName("vietnamese")]
            public PlantDiagnosisLanguageDto? Vietnamese { get; set; }

            [JsonPropertyName("englishDisease")]
            public string? EnglishDisease { get; set; }

            [JsonPropertyName("englishDescription")]
            public string? EnglishDescription { get; set; }
        }
    }
}