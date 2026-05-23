using System.Text.Json;
using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.BLL.Realtime;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.IotDatas;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class SensorDataService : ISensorDataService
    {
        private const double TemperatureDelta = 0.5;
        private const double HumidityDelta = 2.0;
        private const double SoilMoistureDelta = 3.0;
        private const double LightDelta = 50.0;
        private static readonly TimeSpan HeartbeatInterval = TimeSpan.FromMinutes(30);

        private readonly IIotDataRepository _dataRepo;
        private readonly IIotDeviceRepository _deviceRepo;
        private readonly ISeasonRepository _seasonRepo;
        private readonly IIotRealtime _realtime;

        public SensorDataService(
            IIotDataRepository dataRepo,
            IIotDeviceRepository deviceRepo,
            ISeasonRepository seasonRepo,
            IIotRealtime realtime)
        {
            _dataRepo = dataRepo;
            _deviceRepo = deviceRepo;
            _seasonRepo = seasonRepo;
            _realtime = realtime;
        }

        public async Task<SensorDataResponse> ProcessSensorDataAsync(IotDevice device, SensorDataRequest request)
        {
            device.LastActiveAt = DateTimeHelper.VnNow();

            var rangeErrors = SensorRangeValidator.Validate(
                request.Temperature, request.Humidity, request.SoilMoisture, request.Light);
            if (rangeErrors.Count > 0)
            {
                await _deviceRepo.SaveChangesAsync();
                return new SensorDataResponse
                {
                    Id = Guid.Empty,
                    IsAlert = true,
                    Message = "Dữ liệu cảm biến nằm ngoài khoảng cho phép: " + string.Join(" | ", rangeErrors)
                };
            }

            var recordedAt = request.Timestamp == default
                ? DateTime.UtcNow
                : DateTime.SpecifyKind(request.Timestamp, DateTimeKind.Utc);

            var latest = await _dataRepo.GetLatestByDeviceIdAsync(device.DeviceId);
            var persisted = false;
            Guid? persistedId = null;

            if (ShouldPersist(latest, request, recordedAt))
            {
                var seasonId = await FindActiveSeasonIdAsync(device);

                var iotData = new IotData
                {
                    SensorDataId = Guid.NewGuid(),
                    DeviceId = device.DeviceId,
                    SeasonId = seasonId,
                    RecordedAt = recordedAt,
                    Temperature = request.Temperature,
                    Humidity = request.Humidity,
                    SoilMoisture = request.SoilMoisture,
                    Light = request.Light,
                    IsRaining = request.IsRaining,
                    IsAlert = false,
                    RawData = JsonSerializer.Serialize(request),
                    CreatedAt = DateTimeHelper.VnNow()
                };

                await _dataRepo.AddAsync(iotData);
                persisted = true;
                persistedId = iotData.SensorDataId;
            }

            await _deviceRepo.SaveChangesAsync();

            var farmId = device.Bed?.Plot?.FarmId;
            if (farmId.HasValue)
            {
                await _realtime.PushSensorDataAsync(farmId.Value, device.DeviceId, new
                {
                    sensorDataId = persistedId,
                    deviceId = device.DeviceId,
                    deviceCode = device.DeviceCode,
                    recordedAt,
                    temperature = request.Temperature,
                    humidity = request.Humidity,
                    soilMoisture = request.SoilMoisture,
                    light = request.Light,
                    isRaining = request.IsRaining,
                    isAlert = false,
                    persisted
                });
            }

            return new SensorDataResponse
            {
                Id = persistedId ?? Guid.Empty,
                IsAlert = false,
                Message = persisted ? "Data persisted" : "Data accepted (no significant change)"
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

        private static bool ShouldPersist(IotData? latest, SensorDataRequest request, DateTime recordedAt)
        {
            if (latest == null) return true;

            if (latest.RecordedAt == null) return true;

            var elapsed = recordedAt - latest.RecordedAt.Value;
            if (elapsed >= HeartbeatInterval) return true;

            if (ExceedsDelta(latest.Temperature, request.Temperature, TemperatureDelta)) return true;
            if (ExceedsDelta(latest.Humidity, request.Humidity, HumidityDelta)) return true;
            if (ExceedsDelta(latest.SoilMoisture, request.SoilMoisture, SoilMoistureDelta)) return true;
            if (ExceedsDelta(latest.Light, request.Light, LightDelta)) return true;
            if (latest.IsRaining != request.IsRaining) return true;

            return false;
        }

        private static bool ExceedsDelta(double? previous, double? current, double delta)
        {
            if (!previous.HasValue && !current.HasValue) return false;
            if (!previous.HasValue || !current.HasValue) return true;
            return Math.Abs(current.Value - previous.Value) >= delta;
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
    }
}
