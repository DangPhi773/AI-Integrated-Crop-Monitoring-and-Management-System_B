using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class RecommendationTaskService : IRecommendationTaskService
    {
        private readonly IRecommendationTaskRepository _repo;
        public RecommendationTaskService(IRecommendationTaskRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<RecommendationTaskResponse>>> GetAllAsync()
        {
            var tasks = await _repo.GetAllAsync();
            return new ApiResponse<IEnumerable<RecommendationTaskResponse>>
            {
                Success = true,
                Data = RecommendationTaskMapper.ToResponseList(tasks)
            };
        }

        public async Task<ApiResponse<RecommendationTaskResponse>> GetByIdAsync(Guid id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return new ApiResponse<RecommendationTaskResponse> { Success = false, Message = "Task không tồn tại" };
            return new ApiResponse<RecommendationTaskResponse> { Success = true, Data = RecommendationTaskMapper.ToResponse(task) };
        }

        public async Task<ApiResponse<string>> CreateAsync(RecommendationTaskRequest request)
        {
            var task = new RecommendationTask
            {
                RecommendationTaskId = Guid.NewGuid(),
                CreatedByOwnerId = request.CreatedByOwnerId,
                AssignedToWorkerId = request.AssignedToWorkerId,
                Title = request.Title,
                TaskScheduledAt = request.TaskScheduledAt,
                TaskStatus = request.TaskStatus ?? "Pending",
                TaskCreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(task);
            return await _repo.SaveChangesAsync()
                ? new ApiResponse<string> { Success = true, Message = "Tạo task thành công" }
                : new ApiResponse<string> { Success = false, Message = "Lỗi lưu task" };
        }

        public async Task<ApiResponse<string>> UpdateAsync(Guid id, RecommendationTaskRequest request)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return new ApiResponse<string> { Success = false, Message = "Task không tồn tại" };

            task.Title = request.Title;
            task.AssignedToWorkerId = request.AssignedToWorkerId;
            task.TaskScheduledAt = request.TaskScheduledAt;
            task.TaskStatus = request.TaskStatus;

            _repo.Update(task);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Cập nhật task thành công" };
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return new ApiResponse<string> { Success = false, Message = "Task không tồn tại" };

            if (task.RecommendationTaskDetails.Any())
                return new ApiResponse<string> { Success = false, Message = "Không thể xóa task đã có chi tiết thực hiện!" };

            _repo.Delete(task);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Xóa task thành công" };
        }
    }
}
