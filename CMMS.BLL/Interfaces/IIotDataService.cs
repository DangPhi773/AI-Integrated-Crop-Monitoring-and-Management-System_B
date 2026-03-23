using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.IotDatas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IIotDataService
    {
        Task<ApiResponse<IEnumerable<IotDataResponse>>> GetAllDataAsync();
        Task<ApiResponse<IotDataResponse>> GetDataByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<IotDataResponse>>> GetDataByDeviceIdAsync(Guid deviceId);
        Task<ApiResponse<string>> CreateDataAsync(IotDataRequest request);
        Task<ApiResponse<string>> DeleteDataAsync(Guid id);
    }
}
