using CMMS.BLL.Interfaces;
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
                var data = devices.Select(MapToResponse);
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
                return new ApiResponse<IotDeviceResponse> { Success = true, Data = MapToResponse(device) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IotDeviceResponse> { Success = false, Message = "Error fetching device", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateDeviceAsync(IotDeviceRequest request)
        {
            try
            {
                var entity = new IotDevice
                {
                    DeviceId = Guid.NewGuid(),
                    BedId = request.BedId,
                    Name = request.Name ?? "",
                    Type = request.Type,
                    Status = request.Status ?? "Active",
                    InstallationDate = request.InstallationDate,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    CreatedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(entity);
                if (await _repo.SaveChangesAsync())
                    return new ApiResponse<string> { Success = true, Message = "Device created" };

                return new ApiResponse<string> { Success = false, Message = "Failed to save device" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating device", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateDeviceAsync(Guid id, IotDeviceRequest request)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Device not found" };

                entity.BedId = request.BedId ?? entity.BedId;
                entity.Name = request.Name ?? entity.Name;
                entity.Type = request.Type ?? entity.Type;
                entity.Status = request.Status ?? entity.Status;
                entity.InstallationDate = request.InstallationDate ?? entity.InstallationDate;
                entity.Latitude = request.Latitude ?? entity.Latitude;
                entity.Longitude = request.Longitude ?? entity.Longitude;
                entity.UpdatedAt = DateTime.UtcNow;

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

        private static IotDeviceResponse MapToResponse(IotDevice d) => new IotDeviceResponse
        {
            DeviceId = d.DeviceId,
            BedId = d.BedId,
            Name = d.Name,
            Type = d.Type,
            Status = d.Status,
            InstallationDate = d.InstallationDate,
            Latitude = d.Latitude,
            Longitude = d.Longitude,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt
        };
    }
}
