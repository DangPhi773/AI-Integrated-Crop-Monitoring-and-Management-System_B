using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Maps;
using CMMS.DAL.Interfaces;
using Microsoft.Extensions.Options;

namespace CMMS.BLL.Services
{
    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleMapsSettings _settings;
        private readonly IFarmRepository _farmRepository;

        public GoogleMapsService(HttpClient httpClient, IOptions<GoogleMapsSettings> options, IFarmRepository farmRepository)
        {
            _httpClient = httpClient;
            _settings = options.Value;
            _farmRepository = farmRepository;
        }

        public async Task<GeocodeResultDto> GeocodeAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Địa chỉ không được để trống.");

            EnsureApiKey();

            var url = $"{_settings.BaseUrl.TrimEnd('/')}/geocode/json" +
                      $"?address={Uri.EscapeDataString(address)}" +
                      $"&language={_settings.Language}" +
                      $"&region={_settings.Region}" +
                      $"&key={_settings.ApiKey}";

            return await CallGeocodeAsync(url);
        }

        public async Task<GeocodeResultDto> ReverseGeocodeAsync(decimal latitude, decimal longitude)
        {
            EnsureApiKey();
            ValidateCoordinates(latitude, longitude);

            var lat = latitude.ToString(CultureInfo.InvariantCulture);
            var lng = longitude.ToString(CultureInfo.InvariantCulture);

            var url = $"{_settings.BaseUrl.TrimEnd('/')}/geocode/json" +
                      $"?latlng={lat},{lng}" +
                      $"&language={_settings.Language}" +
                      $"&key={_settings.ApiKey}";

            return await CallGeocodeAsync(url);
        }

        public async Task<GeocodeResultDto> UpdateFarmCoordinatesAsync(Guid farmId, UpdateFarmCoordinatesRequestDto request)
        {
            var farm = await _farmRepository.GetByIdAsync(farmId)
                ?? throw new KeyNotFoundException("Không tìm thấy trang trại.");

            GeocodeResultDto result;

            if (request.Latitude.HasValue && request.Longitude.HasValue)
            {
                result = await ReverseGeocodeAsync(request.Latitude.Value, request.Longitude.Value);
                farm.Latitude = request.Latitude.Value;
                farm.Longitude = request.Longitude.Value;
            }
            else if (!string.IsNullOrWhiteSpace(request.Address))
            {
                result = await GeocodeAsync(request.Address);
                farm.Latitude = result.Latitude;
                farm.Longitude = result.Longitude;
            }
            else
            {
                throw new ArgumentException("Cần truyền địa chỉ hoặc cặp toạ độ.");
            }

            if (!string.IsNullOrWhiteSpace(result.FormattedAddress))
                farm.FarmLocation = result.FormattedAddress;

            _farmRepository.Update(farm);
            await _farmRepository.SaveChangesAsync();

            return result;
        }

        private async Task<GeocodeResultDto> CallGeocodeAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<GoogleGeocodeResponse>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            }) ?? throw new Exception("Phản hồi Google Maps không hợp lệ.");

            if (!string.Equals(payload.Status, "OK", StringComparison.OrdinalIgnoreCase) || payload.Results.Count == 0)
                throw new Exception($"Google Maps trả về lỗi: {payload.Status} {payload.ErrorMessage}".Trim());

            var first = payload.Results[0];
            return new GeocodeResultDto
            {
                FormattedAddress = first.FormattedAddress ?? string.Empty,
                Latitude = (decimal)first.Geometry.Location.Lat,
                Longitude = (decimal)first.Geometry.Location.Lng,
                PlaceId = first.PlaceId,
                LocationType = first.Geometry.LocationType
            };
        }

        private void EnsureApiKey()
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                throw new Exception("Thiếu Google Maps API key.");
        }

        private static void ValidateCoordinates(decimal latitude, decimal longitude)
        {
            if (latitude < -90m || latitude > 90m)
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude phải trong [-90, 90].");
            if (longitude < -180m || longitude > 180m)
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude phải trong [-180, 180].");
        }

        private class GoogleGeocodeResponse
        {
            [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;
            [JsonPropertyName("error_message")] public string? ErrorMessage { get; set; }
            [JsonPropertyName("results")] public List<GoogleGeocodeResult> Results { get; set; } = new();
        }

        private class GoogleGeocodeResult
        {
            [JsonPropertyName("formatted_address")] public string? FormattedAddress { get; set; }
            [JsonPropertyName("place_id")] public string? PlaceId { get; set; }
            [JsonPropertyName("geometry")] public GoogleGeometry Geometry { get; set; } = new();
        }

        private class GoogleGeometry
        {
            [JsonPropertyName("location")] public GoogleLocation Location { get; set; } = new();
            [JsonPropertyName("location_type")] public string? LocationType { get; set; }
        }

        private class GoogleLocation
        {
            [JsonPropertyName("lat")] public double Lat { get; set; }
            [JsonPropertyName("lng")] public double Lng { get; set; }
        }
    }
}
