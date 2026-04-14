using System.Text.Json;
using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.IotDatas;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class SensorDataService : ISensorDataService
    {
        private readonly IIotDataRepository _dataRepo;
        private readonly IIotDeviceRepository _deviceRepo;
        private readonly ISeasonRepository _seasonRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly IUserRepository _userRepo;

        public SensorDataService(
            IIotDataRepository dataRepo,
            IIotDeviceRepository deviceRepo,
            ISeasonRepository seasonRepo,
            INotificationRepository notificationRepo,
            IUserRepository userRepo)
        {
            _dataRepo = dataRepo;
            _deviceRepo = deviceRepo;
            _seasonRepo = seasonRepo;
            _notificationRepo = notificationRepo;
            _userRepo = userRepo;
        }

        public async Task<SensorDataResponse> ProcessSensorDataAsync(SensorDataRequest request)
        {
            var device = await _deviceRepo.GetByDeviceCodeAsync(request.DeviceId)
                ?? throw new KeyNotFoundException($"Device with code '{request.DeviceId}' not found");

            device.LastActiveAt = DateTimeHelper.VnNow();

            Guid? seasonId = await FindActiveSeasonIdAsync(device);

            bool isAlert = CheckAlert(device.AlertConfigJson, request);

            var iotData = new IotData
            {
                SensorDataId = Guid.NewGuid(),
                DeviceId = device.DeviceId,
                SeasonId = seasonId,
                RecordedAt = DateTime.SpecifyKind(request.Timestamp, DateTimeKind.Utc),
                Temperature = request.Temperature,
                Humidity = request.Humidity,
                SoilMoisture = request.SoilMoisture,
                Light = request.Light,
                IsRaining = request.IsRaining,
                IsAlert = isAlert,
                RawData = JsonSerializer.Serialize(request),
                CreatedAt = DateTimeHelper.VnNow()
            };

            await _dataRepo.AddAsync(iotData);
            await _deviceRepo.SaveChangesAsync();

            if (isAlert)
                await CreateAlertNotificationAsync(device, request);

            return new SensorDataResponse
            {
                Id = iotData.SensorDataId,
                IsAlert = isAlert,
                Message = "Data received successfully"
            };
        }

        public async Task<ApiResponse<IotDataResponse>> GetLatestAsync(string deviceCode)
        {
            var device = await _deviceRepo.GetByDeviceCodeAsync(deviceCode);
            if (device == null)
                return new ApiResponse<IotDataResponse> { Success = false, Message = "Device not found" };

            var data = await _dataRepo.GetLatestByDeviceIdAsync(device.DeviceId);
            if (data == null)
                return new ApiResponse<IotDataResponse> { Success = false, Message = "No data found" };

            return new ApiResponse<IotDataResponse> { Success = true, Data = IotDataMapper.ToResponse(data) };
        }

        public async Task<ApiResponse<IEnumerable<IotDataResponse>>> GetHistoryAsync(string deviceCode, DateTime from, DateTime to)
        {
            var device = await _deviceRepo.GetByDeviceCodeAsync(deviceCode);
            if (device == null)
                return new ApiResponse<IEnumerable<IotDataResponse>> { Success = false, Message = "Device not found" };

            var fromUtc = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            var toUtc = DateTime.SpecifyKind(to, DateTimeKind.Utc);

            var data = await _dataRepo.GetHistoryAsync(device.DeviceId, fromUtc, toUtc);
            var result = data.Select(IotDataMapper.ToResponse);

            return new ApiResponse<IEnumerable<IotDataResponse>> { Success = true, Data = result };
        }

        public async Task<ApiResponse<IEnumerable<IotDataResponse>>> GetAlertsByFarmIdAsync(Guid farmId)
        {
            var data = await _dataRepo.GetAlertsByFarmIdAsync(farmId);
            var result = data.Select(IotDataMapper.ToResponse);

            return new ApiResponse<IEnumerable<IotDataResponse>> { Success = true, Data = result };
        }

        private async Task<Guid?> FindActiveSeasonIdAsync(IotDevice device)
        {
            if (device.BedId == null || device.Bed == null) return null;

            var plot = device.Bed.Plot;
            if (plot?.FarmId == null) return null;

            var seasons = await _seasonRepo.GetAllAsync();
            var activeSeason = seasons.FirstOrDefault(s =>
                s.FarmId == plot.FarmId
                && s.Status != null
                && s.Status.Equals("Active", StringComparison.OrdinalIgnoreCase));

            return activeSeason?.SeasonId;
        }

        private static bool CheckAlert(string? alertConfigJson, SensorDataRequest request)
        {
            if (string.IsNullOrEmpty(alertConfigJson)) return false;

            try
            {
                var config = JsonSerializer.Deserialize<Dictionary<string, AlertThreshold>>(alertConfigJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (config == null) return false;

                if (request.Temperature.HasValue && config.TryGetValue("temperature", out var tempConfig))
                {
                    if (request.Temperature < tempConfig.Min || request.Temperature > tempConfig.Max)
                        return true;
                }

                if (request.Humidity.HasValue && config.TryGetValue("humidity", out var humConfig))
                {
                    if (request.Humidity < humConfig.Min || request.Humidity > humConfig.Max)
                        return true;
                }

                if (request.SoilMoisture.HasValue && config.TryGetValue("soil_moisture", out var soilConfig))
                {
                    if (request.SoilMoisture < soilConfig.Min || request.SoilMoisture > soilConfig.Max)
                        return true;
                }

                if (request.Light.HasValue && config.TryGetValue("light", out var lightConfig))
                {
                    if (request.Light < lightConfig.Min || request.Light > lightConfig.Max)
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private async System.Threading.Tasks.Task CreateAlertNotificationAsync(IotDevice device, SensorDataRequest request)
        {
            var alertDetails = BuildAlertDetails(device.AlertConfigJson, request);

            var owners = await _userRepo.GetByRoleNamesAsync("Owner");
            if (!owners.Any()) return;

            var notifications = owners.Select(owner => new Notification
            {
                NoteId = Guid.NewGuid(),
                UserId = owner.UserId,
                NoteType = "iot_alert",
                NoteTitle = $"Cảnh báo IoT: {alertDetails}",
                NoteMessage = $"Thiết bị {device.Name} ({device.DeviceCode}) ghi nhận chỉ số vượt ngưỡng: {alertDetails}",
                NoteStatus = "unread",
                NoteCreatedAt = DateTimeHelper.VnNow()
            }).ToList();

            await _notificationRepo.AddRangeAsync(notifications);
            await _notificationRepo.SaveChangesAsync();
        }

        private static string BuildAlertDetails(string? alertConfigJson, SensorDataRequest request)
        {
            if (string.IsNullOrEmpty(alertConfigJson)) return "Unknown";

            var alerts = new List<string>();

            try
            {
                var config = JsonSerializer.Deserialize<Dictionary<string, AlertThreshold>>(alertConfigJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (config == null) return "Unknown";

                if (request.Temperature.HasValue && config.TryGetValue("temperature", out var tc))
                {
                    if (request.Temperature < tc.Min || request.Temperature > tc.Max)
                        alerts.Add($"Nhiệt độ: {request.Temperature}°C");
                }

                if (request.Humidity.HasValue && config.TryGetValue("humidity", out var hc))
                {
                    if (request.Humidity < hc.Min || request.Humidity > hc.Max)
                        alerts.Add($"Độ ẩm: {request.Humidity}%");
                }

                if (request.SoilMoisture.HasValue && config.TryGetValue("soil_moisture", out var sc))
                {
                    if (request.SoilMoisture < sc.Min || request.SoilMoisture > sc.Max)
                        alerts.Add($"Độ ẩm đất: {request.SoilMoisture}%");
                }

                if (request.Light.HasValue && config.TryGetValue("light", out var lc))
                {
                    if (request.Light < lc.Min || request.Light > lc.Max)
                        alerts.Add($"Ánh sáng: {request.Light} lux");
                }
            }
            catch { }

            return alerts.Any() ? string.Join(", ", alerts) : "Unknown";
        }

        private class AlertThreshold
        {
            public double Min { get; set; }
            public double Max { get; set; }
        }
    }
}
