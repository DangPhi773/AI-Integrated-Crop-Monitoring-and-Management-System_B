using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.PlantNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CMMS.BLL.Services
{
    public class PlantNetService : IPlantNetService
    {
        private readonly HttpClient _httpClient;
        private readonly PlantNetSettings _settings;

        public PlantNetService(HttpClient httpClient, IOptions<PlantNetSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<PlantNetDiseaseResponse> IdentifyDiseaseAsync(IFormFile image, string organ = "auto")
        {
            var url = $"{_settings.BaseUrl}/diseases/identify?include-related-images=false&lang={_settings.DefaultLang}&api-key={_settings.ApiKey}";

            using var content = new MultipartFormDataContent();
            await using var stream = image.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
            content.Add(fileContent, "images", image.FileName);
            content.Add(new StringContent(organ), "organs");

            var response = await _httpClient.PostAsync(url, content);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new PlantNetDiseaseResponse
                {
                    Results = new List<PlantNetDiseaseResult>(),
                    RemainingRequests = 0,
                    Version = $"Error: {response.StatusCode}"
                };
            }

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var results = new List<PlantNetDiseaseResult>();

            if (root.TryGetProperty("results", out var resultsElement))
            {
                foreach (var item in resultsElement.EnumerateArray())
                {
                    var score = item.TryGetProperty("score", out var s) ? s.GetDouble() : 0;
                    var name = item.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "";
                    var label = "";
                    if (item.TryGetProperty("disease", out var disease) && disease.TryGetProperty("commonNames", out var names))
                    {
                        var namesList = names.EnumerateArray().Select(x => x.GetString()).Where(x => x != null).ToList();
                        label = namesList.FirstOrDefault() ?? name;
                    }
                    if (string.IsNullOrEmpty(label)) label = name;

                    var categories = new List<string>();
                    if (item.TryGetProperty("disease", out var d2) && d2.TryGetProperty("categories", out var cats))
                    {
                        categories = cats.EnumerateArray().Select(x => x.GetString() ?? "").ToList();
                    }

                    results.Add(new PlantNetDiseaseResult
                    {
                        Name = name,
                        Label = label,
                        Score = score,
                        Severity = score >= 0.7 ? "HIGH" : score >= 0.4 ? "MEDIUM" : "LOW",
                        Categories = categories
                    });
                }
            }

            int remaining = 0;
            if (root.TryGetProperty("remainingIdentificationRequests", out var rem))
                remaining = rem.GetInt32();

            var version = root.TryGetProperty("version", out var ver) ? ver.GetString() : null;

            return new PlantNetDiseaseResponse
            {
                Results = results,
                RemainingRequests = remaining,
                Version = version
            };
        }
    }
}
