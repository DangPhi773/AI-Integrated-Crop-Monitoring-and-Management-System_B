using CMMS.BLL.Interfaces;
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
    public class CropGrowthTaskService : ICropGrowthTaskService
    {
        private readonly ICropGrowthTaskRepository _repo;

        public CropGrowthTaskService(ICropGrowthTaskRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<CropGrowthTaskResponse>>> GetAllAsync()
        {
            try
            {
                var tasks = await _repo.GetAllAsync();
                return new ApiResponse<IEnumerable<CropGrowthTaskResponse>>
                {
                    Success = true,
                    Data = tasks.Select(MapToResponse)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CropGrowthTaskResponse>> { Success = false, Message = "Lỗi lấy danh sách", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<CropGrowthTaskResponse>> GetByIdAsync(Guid id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return new ApiResponse<CropGrowthTaskResponse> { Success = false, Message = "Không tìm thấy mẫu công việc." };
            return new ApiResponse<CropGrowthTaskResponse> { Success = true, Data = MapToResponse(task) };
        }

        public async Task<ApiResponse<IEnumerable<CropGrowthTaskResponse>>> GetByStageIdAsync(Guid stageId)
        {
            var tasks = await _repo.GetByStageIdAsync(stageId);
            return new ApiResponse<IEnumerable<CropGrowthTaskResponse>> { Success = true, Data = tasks.Select(MapToResponse) };
        }

        public async Task<ApiResponse<string>> CreateAsync(CropGrowthTaskRequest request)
        {
            try
            {
                var entity = new CropGrowthTask
                {
                    GrowthTaskId = Guid.NewGuid(),
                    StageId = request.StageId,
                    TaskName = request.TaskName,
                    TaskDescription = request.TaskDescription,
                    Frequency = request.Frequency,
                    DurationMinutes = request.DurationMinutes,
                    RequiredTools = request.RequiredTools,
                    RequiredMaterials = request.RequiredMaterials,
                    QuantityPerUnit = request.QuantityPerUnit,
                    QuantityUnit = request.QuantityUnit,
                    Priority = request.Priority,
                    IsMandatory = request.IsMandatory,
                    Notes = request.Notes,
                    CreatedAt = DateTime.UtcNow 
                };

                await _repo.AddAsync(entity);
                var isSaved = await _repo.SaveChangesAsync(); 

                return isSaved
                    ? new ApiResponse<string> { Success = true, Message = "Tạo mẫu công việc thành công." }
                    : new ApiResponse<string> { Success = false, Message = "Lưu dữ liệu thất bại." };
            }
            catch (Exception ex)
            {
                //return new ApiResponse<string> { Success = false, Message = "Lỗi hệ thống", Errors = new List<string> { ex.Message } };
                var realError = ex.InnerException?.Message ?? ex.Message;
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Lỗi Database chi tiết",
                    Errors = new List<string> { realError }
                };
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(Guid id, CropGrowthTaskRequest request)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy dữ liệu." };

                entity.StageId = request.StageId;
                entity.TaskName = request.TaskName;
                entity.TaskDescription = request.TaskDescription;
                entity.Frequency = request.Frequency;
                entity.DurationMinutes = request.DurationMinutes;
                entity.RequiredTools = request.RequiredTools;
                entity.RequiredMaterials = request.RequiredMaterials;
                entity.QuantityPerUnit = request.QuantityPerUnit;
                entity.QuantityUnit = request.QuantityUnit;
                entity.Priority = request.Priority;
                entity.IsMandatory = request.IsMandatory;
                entity.Notes = request.Notes;

                _repo.Update(entity);
                var isSaved = await _repo.SaveChangesAsync(); 

                return isSaved
                    ? new ApiResponse<string> { Success = true, Message = "Cập nhật thành công." }
                    : new ApiResponse<string> { Success = false, Message = "Không có thay đổi nào được lưu." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi cập nhật", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return new ApiResponse<string> { Success = false, Message = "Dữ liệu không tồn tại." };

            _repo.Delete(entity);
            await _repo.SaveChangesAsync(); 
            return new ApiResponse<string> { Success = true, Message = "Xóa thành công." };
        }

        private static CropGrowthTaskResponse MapToResponse(CropGrowthTask t) => new CropGrowthTaskResponse
        {
            GrowthTaskId = t.GrowthTaskId,
            StageId = t.StageId,
            StageName = t.CropGrowthStage?.StageName,
            TaskName = t.TaskName,
            TaskDescription = t.TaskDescription,
            Frequency = t.Frequency,
            DurationMinutes = t.DurationMinutes,
            RequiredTools = t.RequiredTools,
            RequiredMaterials = t.RequiredMaterials,
            QuantityPerUnit = t.QuantityPerUnit,
            QuantityUnit = t.QuantityUnit,
            Priority = t.Priority,
            IsMandatory = t.IsMandatory,
            Notes = t.Notes,
            CreatedAt = DateTime.SpecifyKind(t.CreatedAt, DateTimeKind.Utc)
        };

        public async Task<bool> SaveAsync() => await _repo.SaveChangesAsync();
    }
}
