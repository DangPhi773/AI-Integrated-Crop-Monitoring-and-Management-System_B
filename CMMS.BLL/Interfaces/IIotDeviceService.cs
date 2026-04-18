using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.IotDevices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IIotDeviceService
    {
        Task<ApiResponse<IEnumerable<IotDeviceResponse>>> GetAllDevicesAsync();
        Task<ApiResponse<IotDeviceResponse>> GetDeviceByIdAsync(Guid id);
        Task<ApiResponse<IotDeviceCreatedResponse>> CreateDeviceAsync(IotDeviceRequest request);
        Task<ApiResponse<IotDeviceCreatedResponse>> RegenerateApiKeyAsync(Guid id);
        Task<ApiResponse<string>> UpdateDeviceAsync(Guid id, IotDeviceRequest request);
        Task<ApiResponse<string>> DeleteDeviceAsync(Guid id);
    }
}
