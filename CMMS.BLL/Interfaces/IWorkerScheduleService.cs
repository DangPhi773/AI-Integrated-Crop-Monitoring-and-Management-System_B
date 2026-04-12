using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.WorkerSchedules;

namespace CMMS.BLL.Interfaces
{
    public interface IWorkerScheduleService
    {
        Task<ApiResponse<IEnumerable<WorkerScheduleResponse>>> GetByWorkerIdAsync(Guid workerId);
        Task<ApiResponse<IEnumerable<WorkerScheduleResponse>>> GetByTaskDetailIdAsync(Guid taskDetailId);
        Task<ApiResponse<IEnumerable<WorkerScheduleResponse>>> GetMyScheduleAsync(Guid workerId);
    }
}
