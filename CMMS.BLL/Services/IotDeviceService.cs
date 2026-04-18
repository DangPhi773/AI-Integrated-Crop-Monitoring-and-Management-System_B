using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.IotDevices;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class IotDeviceService : IIotDeviceService
    {
        private readonly IIotDeviceRepository _repo;

        public IotDeviceService(IIotDeviceRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<IotDeviceResponse>>> GetAllDevicesAsync()
        {
            try
            {
                var devices = await _repo.GetAllAsync();
                var data = devices.Select(IotDeviceMapper.ToResponse);
                return new ApiResponse<IEnumerable<IotDeviceResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<IotDeviceResponse>> { Success = false, Message = "Error fetching devices", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IotDeviceResponse>> GetDeviceByIdAsync(Guid id)
        {
            try
            {
                var device = await _repo.GetByIdAsync(id);
                if (device == null) return new ApiResponse<IotDeviceResponse> { Success = false, Message = "Device not found" };
                return new ApiResponse<IotDeviceResponse> { Success = true, Data = IotDeviceMapper.ToResponse(device) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IotDeviceResponse> { Success = false, Message = "Error fetching device", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IotDeviceCreatedResponse>> CreateDeviceAsync(IotDeviceRequest request)
        {
            try
            {
                var plainKey = DeviceApiKeyHelper.GenerateKey();
                var keyHash = DeviceApiKeyHelper.HashKey(plainKey);
                var now = DateTimeHelper.VnNow();

                var entity = new IotDevice
                {
                    DeviceId = Guid.NewGuid(),
                    BedId = request.BedId,
                    DeviceCode = request.DeviceCode,
                    Name = request.Name ?? "",
                    Type = request.Type,
                    Status = request.Status ?? "Active",
                    InstallationDate = request.InstallationDate,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    CreatedAt = now,
                    ApiKeyHash = keyHash,
                    ApiKeyRotatedAt = now
                };

                await _repo.AddAsync(entity);
                if (!await _repo.SaveChangesAsync())
                    return new ApiResponse<IotDeviceCreatedResponse> { Success = false, Message = "Failed to save device" };

                return new ApiResponse<IotDeviceCreatedResponse>
                {
                    Success = true,
                    Message = "Device created",
                    Data = new IotDeviceCreatedResponse
                    {
                        DeviceId = entity.DeviceId,
                        DeviceCode = entity.DeviceCode,
                        Name = entity.Name,
                        ApiKey = plainKey
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IotDeviceCreatedResponse> { Success = false, Message = "Error creating device", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IotDeviceCreatedResponse>> RegenerateApiKeyAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<IotDeviceCreatedResponse> { Success = false, Message = "Device not found" };

                var plainKey = DeviceApiKeyHelper.GenerateKey();
                entity.ApiKeyHash = DeviceApiKeyHelper.HashKey(plainKey);
                entity.ApiKeyRotatedAt = DateTimeHelper.VnNow();
                entity.UpdatedAt = entity.ApiKeyRotatedAt;

                _repo.Update(entity);
                if (!await _repo.SaveChangesAsync())
                    return new ApiResponse<IotDeviceCreatedResponse> { Success = false, Message = "Failed to rotate key" };

                return new ApiResponse<IotDeviceCreatedResponse>
                {
                    Success = true,
                    Message = "Device key regenerated",
                    Data = new IotDeviceCreatedResponse
                    {
                        DeviceId = entity.DeviceId,
                        DeviceCode = entity.DeviceCode,
                        Name = entity.Name,
                        ApiKey = plainKey
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IotDeviceCreatedResponse> { Success = false, Message = "Error rotating key", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateDeviceAsync(Guid id, IotDeviceRequest request)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Device not found" };

                entity.BedId = request.BedId ?? entity.BedId;
                entity.DeviceCode = request.DeviceCode ?? entity.DeviceCode;
                entity.Name = request.Name ?? entity.Name;
                entity.Type = request.Type ?? entity.Type;
                entity.Status = request.Status ?? entity.Status;
                entity.InstallationDate = request.InstallationDate ?? entity.InstallationDate;
                entity.Latitude = request.Latitude ?? entity.Latitude;
                entity.Longitude = request.Longitude ?? entity.Longitude;
                entity.UpdatedAt = DateTimeHelper.VnNow();

                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Device updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating device", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteDeviceAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Device not found" };

                _repo.Delete(entity);
                await _repo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Device deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting device", Errors = new List<string> { ex.Message } };
            }
        }
    }
}
