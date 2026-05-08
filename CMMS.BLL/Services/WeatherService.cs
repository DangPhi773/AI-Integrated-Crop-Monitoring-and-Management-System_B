using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Weather;
using CMMS.DAL.Interfaces;
using Microsoft.Extensions.Options;

namespace CMMS.BLL.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherSettings _settings;
        private readonly IFarmRepository _farmRepository;

        public WeatherService(HttpClient httpClient, IOptions<WeatherSettings> options, IFarmRepository farmRepository)
        {
            _httpClient = httpClient;
            _settings = options.Value;
            _farmRepository = farmRepository;
        }

        public async Task<WeatherCurrentDto> GetCurrentAsync(decimal latitude, decimal longitude)
        {
            EnsureApiKey();
            ValidateCoordinates(latitude, longitude);

            var url = BuildUrl("current.json", latitude, longitude);
            var raw = await _httpClient.GetFromJsonAsync<WeatherApiCurrentResponse>(url, JsonOpts)
                ?? throw new Exception("Phản hồi WeatherAPI không hợp lệ.");

            return MapCurrent(raw);
        }

        public async Task<WeatherForecastDto> GetForecastAsync(decimal latitude, decimal longitude, int days)
        {
            EnsureApiKey();
            ValidateCoordinates(latitude, longitude);

            if (days <= 0) days = _settings.DefaultForecastDays;
            if (days > 14) days = 14;

            var url = BuildUrl("forecast.json", latitude, longitude, $"&days={days}&aqi=no&alerts=no");
            var raw = await _httpClient.GetFromJsonAsync<WeatherApiForecastResponse>(url, JsonOpts)
                ?? throw new Exception("Phản hồi WeatherAPI không hợp lệ.");

            return new WeatherForecastDto
            {
                Location = MapLocation(raw.Location),
                Current = MapCurrent(new WeatherApiCurrentResponse { Location = raw.Location, Current = raw.Current }),
                Forecast = raw.Forecast?.ForecastDay?.Select(MapForecastDay).ToList() ?? new List<WeatherForecastDayDto>()
            };
        }

        public async Task<WeatherCurrentDto> GetCurrentByFarmAsync(Guid farmId)
        {
            var (lat, lng) = await GetFarmCoordinatesAsync(farmId);
            return await GetCurrentAsync(lat, lng);
        }

        public async Task<WeatherForecastDto> GetForecastByFarmAsync(Guid farmId, int days)
        {
            var (lat, lng) = await GetFarmCoordinatesAsync(farmId);
            return await GetForecastAsync(lat, lng, days);
        }

        private async Task<(decimal Latitude, decimal Longitude)> GetFarmCoordinatesAsync(Guid farmId)
        {
            var farm = await _farmRepository.GetByIdAsync(farmId)
                ?? throw new KeyNotFoundException("Không tìm thấy trang trại.");

            if (!farm.Latitude.HasValue || !farm.Longitude.HasValue)
                throw new Exception("Trang trại chưa có toạ độ. Hãy cập nhật toạ độ trước khi lấy thời tiết.");

            return (farm.Latitude.Value, farm.Longitude.Value);
        }

        private string BuildUrl(string endpoint, decimal latitude, decimal longitude, string? extra = null)
        {
            var lat = latitude.ToString(CultureInfo.InvariantCulture);
            var lng = longitude.ToString(CultureInfo.InvariantCulture);

            return $"{_settings.BaseUrl.TrimEnd('/')}/{endpoint}" +
                   $"?key={_settings.ApiKey}" +
                   $"&q={lat},{lng}" +
                   $"&lang={_settings.Language}" +
                   (extra ?? string.Empty);
        }

        private void EnsureApiKey()
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                throw new Exception("Thiếu WeatherAPI key.");
        }

        private static void ValidateCoordinates(decimal latitude, decimal longitude)
        {
            if (latitude < -90m || latitude > 90m)
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude phải trong [-90, 90].");
            if (longitude < -180m || longitude > 180m)
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude phải trong [-180, 180].");
        }

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static WeatherLocationDto MapLocation(WeatherApiLocation? loc) => new()
        {
            Name = loc?.Name,
            Region = loc?.Region,
            Country = loc?.Country,
            Latitude = (decimal)(loc?.Lat ?? 0),
            Longitude = (decimal)(loc?.Lon ?? 0),
            LocalTime = loc?.Localtime
        };

        private static WeatherCurrentDto MapCurrent(WeatherApiCurrentResponse raw)
        {
            var c = raw.Current ?? new WeatherApiCurrent();
            return new WeatherCurrentDto
            {
                Location = MapLocation(raw.Location),
                LastUpdated = c.LastUpdatedEpoch > 0
                    ? DateTimeOffset.FromUnixTimeSeconds(c.LastUpdatedEpoch).UtcDateTime
                    : DateTime.UtcNow,
                TempC = c.TempC,
                FeelsLikeC = c.FeelslikeC,
                Humidity = c.Humidity,
                WindKph = c.WindKph,
                WindDir = c.WindDir,
                GustKph = c.GustKph,
                PrecipMm = c.PrecipMm,
                PressureMb = c.PressureMb,
                Cloud = c.Cloud,
                Uv = c.Uv,
                VisKm = c.VisKm,
                IsDay = c.IsDay == 1,
                Condition = new WeatherConditionDto
                {
                    Text = c.Condition?.Text,
                    Icon = c.Condition?.Icon,
                    Code = c.Condition?.Code ?? 0
                }
            };
        }

        private static WeatherForecastDayDto MapForecastDay(WeatherApiForecastDay d) => new()
        {
            Date = DateTime.TryParse(d.Date, out var date) ? date : DateTime.MinValue,
            MaxTempC = d.Day?.MaxtempC ?? 0,
            MinTempC = d.Day?.MintempC ?? 0,
            AvgTempC = d.Day?.AvgtempC ?? 0,
            TotalPrecipMm = d.Day?.TotalprecipMm ?? 0,
            AvgHumidity = d.Day?.Avghumidity ?? 0,
            MaxWindKph = d.Day?.MaxwindKph ?? 0,
            Uv = d.Day?.Uv ?? 0,
            ChanceOfRain = d.Day?.DailyChanceOfRain ?? 0,
            Condition = new WeatherConditionDto
            {
                Text = d.Day?.Condition?.Text,
                Icon = d.Day?.Condition?.Icon,
                Code = d.Day?.Condition?.Code ?? 0
            },
            Sunrise = d.Astro?.Sunrise,
            Sunset = d.Astro?.Sunset
        };

        private class WeatherApiLocation
        {
            public string? Name { get; set; }
            public string? Region { get; set; }
            public string? Country { get; set; }
            public double Lat { get; set; }
            public double Lon { get; set; }
            public string? Localtime { get; set; }
        }

        private class WeatherApiCondition
        {
            public string? Text { get; set; }
            public string? Icon { get; set; }
            public int Code { get; set; }
        }

        private class WeatherApiCurrent
        {
            [JsonPropertyName("last_updated_epoch")] public long LastUpdatedEpoch { get; set; }
            [JsonPropertyName("temp_c")] public double TempC { get; set; }
            [JsonPropertyName("feelslike_c")] public double FeelslikeC { get; set; }
            public int Humidity { get; set; }
            [JsonPropertyName("wind_kph")] public double WindKph { get; set; }
            [JsonPropertyName("wind_dir")] public string? WindDir { get; set; }
            [JsonPropertyName("gust_kph")] public double GustKph { get; set; }
            [JsonPropertyName("precip_mm")] public double PrecipMm { get; set; }
            [JsonPropertyName("pressure_mb")] public double PressureMb { get; set; }
            public int Cloud { get; set; }
            public double Uv { get; set; }
            [JsonPropertyName("vis_km")] public double VisKm { get; set; }
            [JsonPropertyName("is_day")] public int IsDay { get; set; }
            public WeatherApiCondition? Condition { get; set; }
        }

        private class WeatherApiCurrentResponse
        {
            public WeatherApiLocation? Location { get; set; }
            public WeatherApiCurrent? Current { get; set; }
        }

        private class WeatherApiForecastResponse
        {
            public WeatherApiLocation? Location { get; set; }
            public WeatherApiCurrent? Current { get; set; }
            public WeatherApiForecast? Forecast { get; set; }
        }

        private class WeatherApiForecast
        {
            [JsonPropertyName("forecastday")] public List<WeatherApiForecastDay>? ForecastDay { get; set; }
        }

        private class WeatherApiForecastDay
        {
            public string? Date { get; set; }
            public WeatherApiDay? Day { get; set; }
            public WeatherApiAstro? Astro { get; set; }
        }

        private class WeatherApiDay
        {
            [JsonPropertyName("maxtemp_c")] public double MaxtempC { get; set; }
            [JsonPropertyName("mintemp_c")] public double MintempC { get; set; }
            [JsonPropertyName("avgtemp_c")] public double AvgtempC { get; set; }
            [JsonPropertyName("totalprecip_mm")] public double TotalprecipMm { get; set; }
            public int Avghumidity { get; set; }
            [JsonPropertyName("maxwind_kph")] public double MaxwindKph { get; set; }
            public double Uv { get; set; }
            [JsonPropertyName("daily_chance_of_rain")] public int DailyChanceOfRain { get; set; }
            public WeatherApiCondition? Condition { get; set; }
        }

        private class WeatherApiAstro
        {
            public string? Sunrise { get; set; }
            public string? Sunset { get; set; }
        }
    }
}
