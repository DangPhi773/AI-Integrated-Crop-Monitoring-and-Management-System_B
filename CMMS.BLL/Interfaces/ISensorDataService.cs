using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.IotDatas;

namespace CMMS.BLL.Interfaces
{
    public interface ISensorDataService
    {
        Task<SensorDataResponse> ProcessSensorDataAsync(SensorDataRequest request);
        Task<ApiResponse<IotDataResponse>> GetLatestAsync(string deviceCode);
        Task<ApiResponse<IEnumerable<IotDataResponse>>> GetHistoryAsync(string deviceCode, DateTime from, DateTime to);
        Task<ApiResponse<IEnumerable<IotDataResponse>>> GetAlertsByFarmIdAsync(Guid farmId);
    }
}
