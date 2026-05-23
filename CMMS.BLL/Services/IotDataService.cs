using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.IotDatas;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class IotDataService : IIotDataService
    {
        private readonly IIotDataRepository _repo;

        public IotDataService(IIotDataRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<IotDataResponse>>> GetAllDataAsync()
        {
            try
            {
                var data = await _repo.GetAllAsync();
                var result = data.Select(IotDataMapper.ToResponse);
                return new ApiResponse<IEnumerable<IotDataResponse>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<IotDataResponse>> { Success = false, Message = "Error fetching IoT data", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IotDataResponse>> GetDataByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<IotDataResponse> { Success = false, Message = "Data not found" };
                return new ApiResponse<IotDataResponse> { Success = true, Data = IotDataMapper.ToResponse(entity) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IotDataResponse> { Success = false, Message = "Error fetching IoT data", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<IotDataResponse>>> GetDataByDeviceIdAsync(Guid deviceId)
        {
            try
            {
                var data = await _repo.GetByDeviceIdAsync(deviceId);
                var result = data.Select(IotDataMapper.ToResponse);
                return new ApiResponse<IEnumerable<IotDataResponse>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<IotDataResponse>> { Success = false, Message = "Error fetching IoT data by device", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateDataAsync(IotDataRequest request)
        {
            try
            {
                var rangeErrors = SensorRangeValidator.Validate(
                    request.Temperature, request.Humidity, request.SoilMoisture, request.Light);
                if (rangeErrors.Count > 0)
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Dữ liệu cảm biến nằm ngoài khoảng cho phép",
                        Errors = rangeErrors
                    };

                var entity = new IotData
                {
                    SensorDataId = Guid.NewGuid(),
                    DeviceId = request.DeviceId,
                    SeasonId = request.SeasonId,
                    RecordedAt = DateTimeHelper.VnNow(),
                    Temperature = request.Temperature,
                    Humidity = request.Humidity,
                    SoilMoisture = request.SoilMoisture,
                    Light = request.Light,
                    IsRaining = request.IsRaining,
                    IsAlert = false,
                    CreatedAt = DateTimeHelper.VnNow()
                };

                await _repo.AddAsync(entity);
                if (await _repo.SaveChangesAsync())
                    return new ApiResponse<string> { Success = true, Message = "IoT data created" };

                return new ApiResponse<string> { Success = false, Message = "Failed to save IoT data" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating IoT data", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteDataAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Data not found" };

                _repo.Delete(entity);
                await _repo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "IoT data deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting IoT data", Errors = new List<string> { ex.Message } };
            }
        }
    }
}
