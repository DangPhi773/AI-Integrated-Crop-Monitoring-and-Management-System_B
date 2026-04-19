using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.DTOs.Tasks.CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class RecommendationTaskDetailService : IRecommendationTaskDetailService
    {
        private readonly IRecommendationTaskDetailRepository _repo;
        public RecommendationTaskDetailService(IRecommendationTaskDetailRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<RecommendationTaskDetailResponse>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return new ApiResponse<IEnumerable<RecommendationTaskDetailResponse>> { Success = true, Data = RecommendationTaskDetailMapper.ToResponseList(list) };
        }

        public async Task<ApiResponse<IEnumerable<RecommendationTaskDetailResponse>>> GetByTaskIdAsync(Guid taskId)
        {
            var list = await _repo.GetByTaskIdAsync(taskId);
            return new ApiResponse<IEnumerable<RecommendationTaskDetailResponse>> { Success = true, Data = RecommendationTaskDetailMapper.ToResponseList(list) };
        }

        public async Task<ApiResponse<RecommendationTaskDetailResponse>> GetByIdAsync(Guid id)
        {
            var detail = await _repo.GetByIdAsync(id);
            if (detail == null) return new ApiResponse<RecommendationTaskDetailResponse> { Success = false, Message = "Chi tiết công việc không tồn tại" };
            return new ApiResponse<RecommendationTaskDetailResponse> { Success = true, Data = RecommendationTaskDetailMapper.ToResponse(detail) };
        }

        public async Task<ApiResponse<string>> CreateAsync(RecommendationTaskDetailRequest request)
        {
            var detail = new RecommendationTaskDetail
            {
                TaskDetailId = Guid.NewGuid(),
                TaskId = request.TaskId,
                SeasonId = request.SeasonId, 
                FarmId = request.FarmId,    
                Title = request.Title,
                Quantity = request.Quantity,
                Status = request.Status ?? "Pending",
                Unit = request.Unit,
                Notes = request.Notes,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                AssignedToWorkerIds = request.AssignedToWorkerIds ?? new List<Guid>(),
                PlotIds = request.PlotIds ?? new List<Guid>(),
                BedIds = request.BedIds ?? new List<Guid>()
            };

            await _repo.AddAsync(detail);
            return await _repo.SaveChangesAsync()
                ? new ApiResponse<string> { Success = true, Message = "Thêm chi tiết công việc thành công" }
                : new ApiResponse<string> { Success = false, Message = "Lỗi khi lưu dữ liệu" };
        }

        public async Task<ApiResponse<string>> UpdateAsync(Guid id, RecommendationTaskDetailRequest request)
        {
            var detail = await _repo.GetByIdAsync(id);
            if (detail == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy dữ liệu" };

            detail.Title = request.Title;
            detail.SeasonId = request.SeasonId;
            detail.FarmId = request.FarmId;
            detail.Quantity = request.Quantity;
            detail.Status = request.Status;
            detail.Unit = request.Unit;
            detail.Notes = request.Notes;
            detail.StartDate = request.StartDate;
            detail.EndDate = request.EndDate;
            detail.AssignedToWorkerIds = request.AssignedToWorkerIds ?? new List<Guid>();
            detail.PlotIds = request.PlotIds ?? new List<Guid>();
            detail.BedIds = request.BedIds ?? new List<Guid>();

            _repo.Update(detail);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Cập nhật thành công" };
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var detail = await _repo.GetByIdAsync(id);
            if (detail == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy để xóa" };

            _repo.Delete(detail);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Xóa chi tiết công việc thành công" };
        }
    }
}
