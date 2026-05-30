using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
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

            var url = BuildUrl("weather", latitude, longitude);
            var raw = await _httpClient.GetFromJsonAsync<OwCurrentResponse>(url, JsonOpts)
                ?? throw new Exception("Phản hồi OpenWeather không hợp lệ.");

            return MapCurrent(raw);
        }

        public async Task<WeatherForecastDto> GetForecastAsync(decimal latitude, decimal longitude, int days)
        {
            EnsureApiKey();
            ValidateCoordinates(latitude, longitude);

            if (days <= 0) days = _settings.DefaultForecastDays;
            if (days > 5) days = 5;

            var currentUrl = BuildUrl("weather", latitude, longitude);
            var forecastUrl = BuildUrl("forecast", latitude, longitude);

            var currentTask = _httpClient.GetFromJsonAsync<OwCurrentResponse>(currentUrl, JsonOpts);
            var forecastTask = _httpClient.GetFromJsonAsync<OwForecastResponse>(forecastUrl, JsonOpts);
            await Task.WhenAll(currentTask, forecastTask);

            var current = currentTask.Result ?? throw new Exception("Phản hồi OpenWeather không hợp lệ.");
            var forecast = forecastTask.Result ?? throw new Exception("Phản hồi OpenWeather không hợp lệ.");

            return new WeatherForecastDto
            {
                Location = MapLocationFromCurrent(current),
                Current = MapCurrent(current),
                Forecast = AggregateForecast(forecast, days)
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

        private string BuildUrl(string endpoint, decimal latitude, decimal longitude)
        {
            var lat = latitude.ToString(CultureInfo.InvariantCulture);
            var lon = longitude.ToString(CultureInfo.InvariantCulture);

            return $"{_settings.BaseUrl.TrimEnd('/')}/{endpoint}" +
                   $"?lat={lat}&lon={lon}" +
                   $"&appid={_settings.ApiKey}" +
                   $"&units={_settings.Units}" +
                   $"&lang={_settings.Language}";
        }

        private void EnsureApiKey()
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                throw new Exception("Thiếu OpenWeather API key.");
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

        private static WeatherLocationDto MapLocationFromCurrent(OwCurrentResponse raw) => new()
        {
            Name = raw.Name,
            Region = null,
            Country = raw.Sys?.Country,
            Latitude = (decimal)(raw.Coord?.Lat ?? 0),
            Longitude = (decimal)(raw.Coord?.Lon ?? 0),
            LocalTime = ToLocalTimeString(raw.Dt, raw.Timezone)
        };

        private static WeatherCurrentDto MapCurrent(OwCurrentResponse raw)
        {
            var m = raw.Main ?? new OwMain();
            var w = raw.Weather?.FirstOrDefault();
            var wind = raw.Wind ?? new OwWind();
            var precipMm = raw.Rain?.OneHour ?? raw.Snow?.OneHour ?? 0;

            return new WeatherCurrentDto
            {
                Location = MapLocationFromCurrent(raw),
                LastUpdated = raw.Dt > 0
                    ? DateTimeOffset.FromUnixTimeSeconds(raw.Dt).UtcDateTime
                    : DateTime.UtcNow,
                TempC = m.Temp,
                FeelsLikeC = m.FeelsLike,
                Humidity = m.Humidity,
                WindKph = Math.Round(wind.Speed * 3.6, 2),
                WindDir = DegToCompass(wind.Deg),
                GustKph = wind.Gust.HasValue ? Math.Round(wind.Gust.Value * 3.6, 2) : 0,
                PrecipMm = precipMm,
                PressureMb = m.Pressure,
                Cloud = raw.Clouds?.All ?? 0,
                Uv = 0,
                VisKm = (raw.Visibility ?? 0) / 1000.0,
                IsDay = IsDay(raw.Dt, raw.Sys?.Sunrise, raw.Sys?.Sunset),
                Condition = new WeatherConditionDto
                {
                    Text = w?.Description,
                    Icon = w?.Icon,
                    Code = w?.Id ?? 0
                }
            };
        }

        private static List<WeatherForecastDayDto> AggregateForecast(OwForecastResponse raw, int days)
        {
            if (raw.List == null || raw.List.Count == 0)
                return new List<WeatherForecastDayDto>();

            var tz = raw.City?.Timezone ?? 0;
            var citySunrise = raw.City?.Sunrise;
            var citySunset = raw.City?.Sunset;

            var groups = raw.List
                .GroupBy(slot => DateTimeOffset.FromUnixTimeSeconds(slot.Dt + tz).UtcDateTime.Date)
                .OrderBy(g => g.Key)
                .Take(days)
                .ToList();

            var result = new List<WeatherForecastDayDto>();
            DateTime? firstDay = groups.FirstOrDefault()?.Key;

            foreach (var g in groups)
            {
                var slots = g.ToList();
                var noon = slots.OrderBy(s => Math.Abs(((s.Dt + tz) % 86400) - 43200)).First();
                var cond = noon.Weather?.FirstOrDefault();

                var totalPrecip = slots.Sum(s => (s.Rain?.ThreeHour ?? 0) + (s.Snow?.ThreeHour ?? 0));
                var maxPop = slots.Max(s => s.Pop);
                bool isFirstDay = firstDay.HasValue && g.Key == firstDay.Value;

                result.Add(new WeatherForecastDayDto
                {
                    Date = g.Key,
                    MaxTempC = slots.Max(s => s.Main?.TempMax ?? s.Main?.Temp ?? 0),
                    MinTempC = slots.Min(s => s.Main?.TempMin ?? s.Main?.Temp ?? 0),
                    AvgTempC = Math.Round(slots.Average(s => s.Main?.Temp ?? 0), 2),
                    TotalPrecipMm = Math.Round(totalPrecip, 2),
                    AvgHumidity = (int)Math.Round(slots.Average(s => s.Main?.Humidity ?? 0)),
                    MaxWindKph = Math.Round(slots.Max(s => (s.Wind?.Speed ?? 0) * 3.6), 2),
                    Uv = 0,
                    ChanceOfRain = (int)Math.Round(maxPop * 100),
                    Condition = new WeatherConditionDto
                    {
                        Text = cond?.Description,
                        Icon = cond?.Icon,
                        Code = cond?.Id ?? 0
                    },
                    Sunrise = isFirstDay && citySunrise.HasValue
                        ? ToLocalTimeString(citySunrise.Value, tz, "HH:mm")
                        : null,
                    Sunset = isFirstDay && citySunset.HasValue
                        ? ToLocalTimeString(citySunset.Value, tz, "HH:mm")
                        : null
                });
            }

            return result;
        }

        private static string DegToCompass(double deg)
        {
            string[] dirs = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
            int idx = (int)Math.Floor((deg / 45.0) + 0.5) % 8;
            if (idx < 0) idx += 8;
            return dirs[idx];
        }

        private static bool IsDay(long dt, long? sunrise, long? sunset)
        {
            if (!sunrise.HasValue || !sunset.HasValue) return true;
            return dt >= sunrise.Value && dt < sunset.Value;
        }

        private static string? ToLocalTimeString(long epoch, int tzOffset, string? format = null)
        {
            if (epoch <= 0) return null;
            var local = DateTimeOffset.FromUnixTimeSeconds(epoch + tzOffset).UtcDateTime;
            return local.ToString(format ?? "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }
    }
}
