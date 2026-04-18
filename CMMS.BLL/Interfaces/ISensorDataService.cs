using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.IotDatas;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Interfaces
{
    public interface ISensorDataService
    {
        Task<SensorDataResponse> ProcessSensorDataAsync(IotDevice device, SensorDataRequest request);
        Task<ApiResponse<IotDataResponse>> GetLatestAsync(string deviceCode);
        Task<ApiResponse<IEnumerable<IotDataResponse>>> GetHistoryAsync(string deviceCode, DateTime from, DateTime to);
    }
}
