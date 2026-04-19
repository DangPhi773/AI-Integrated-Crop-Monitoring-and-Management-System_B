using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ITaskDetailService
    {
        Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetAllAsync();
        Task<ApiResponse<TaskDetailResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetBySeasonIdAsync(Guid seasonId);
        Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetByWorkerIdAsync(Guid workerId);
        Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetByBedIdAsync(Guid bedId);
        Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetByTaskIdAsync(Guid taskId);
        Task<ApiResponse<string>> CreateAsync(TaskDetailRequest request);
        Task<ApiResponse<string>> UpdateAsync(Guid id, TaskDetailRequest request);
        Task<ApiResponse<string>> UpdateStatusAsync(Guid id, string status, Guid workerId);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
