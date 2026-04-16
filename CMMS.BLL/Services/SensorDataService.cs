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

        public SensorDataService(
            IIotDataRepository dataRepo,
            IIotDeviceRepository deviceRepo,
            ISeasonRepository seasonRepo)
        {
            _dataRepo = dataRepo;
            _deviceRepo = deviceRepo;
            _seasonRepo = seasonRepo;
        }

        public async Task<SensorDataResponse> ProcessSensorDataAsync(SensorDataRequest request)
        {
            var device = await _deviceRepo.GetByDeviceCodeAsync(request.DeviceId)
                ?? throw new KeyNotFoundException($"Device with code '{request.DeviceId}' not found");

            device.LastActiveAt = DateTimeHelper.VnNow();

            Guid? seasonId = await FindActiveSeasonIdAsync(device);

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
                IsAlert = false,
                RawData = JsonSerializer.Serialize(request),
                CreatedAt = DateTimeHelper.VnNow()
            };

            await _dataRepo.AddAsync(iotData);
            await _deviceRepo.SaveChangesAsync();

            return new SensorDataResponse
            {
                Id = iotData.SensorDataId,
                IsAlert = false,
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
