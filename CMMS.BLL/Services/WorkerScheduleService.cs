using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.WorkerSchedules;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class WorkerScheduleService : IWorkerScheduleService
    {
        private readonly IWorkerScheduleRepository _scheduleRepo;

        public WorkerScheduleService(IWorkerScheduleRepository scheduleRepo)
        {
            _scheduleRepo = scheduleRepo;
        }

        public async Task<ApiResponse<IEnumerable<WorkerScheduleResponse>>> GetByWorkerIdAsync(Guid workerId)
        {
            try
            {
                var schedules = await _scheduleRepo.GetByWorkerIdAsync(workerId);
                var data = schedules.Select(WorkerScheduleMapper.ToResponse);
                return new ApiResponse<IEnumerable<WorkerScheduleResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<WorkerScheduleResponse>> { Success = false, Message = "Lỗi lấy lịch làm việc", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<WorkerScheduleResponse>>> GetByTaskDetailIdAsync(Guid taskDetailId)
        {
            try
            {
                var schedules = await _scheduleRepo.GetByTaskDetailIdAsync(taskDetailId);
                var data = schedules.Select(WorkerScheduleMapper.ToResponse);
                return new ApiResponse<IEnumerable<WorkerScheduleResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<WorkerScheduleResponse>> { Success = false, Message = "Lỗi lấy lịch theo task detail", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<WorkerScheduleResponse>>> GetMyScheduleAsync(Guid workerId)
        {
            return await GetByWorkerIdAsync(workerId);
        }
    }
}
