using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class TaskDetailService : ITaskDetailService
    {
        private readonly ITaskDetailRepository _repo;

        public TaskDetailService(ITaskDetailRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetAllAsync()
        {
            try
            {
                var details = await _repo.GetAllAsync();
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(MapToResponse) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = false, Message = "Error fetching task details", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<TaskDetailResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var d = await _repo.GetByIdAsync(id);
                if (d == null) return new ApiResponse<TaskDetailResponse> { Success = false, Message = "Task detail not found" };
                return new ApiResponse<TaskDetailResponse> { Success = true, Data = MapToResponse(d) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TaskDetailResponse> { Success = false, Message = "Error", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetBySeasonIdAsync(Guid seasonId)
        {
            try
            {
                var details = await _repo.GetBySeasonIdAsync(seasonId);
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(MapToResponse) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = false, Message = "Error", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetByWorkerIdAsync(Guid workerId)
        {
            try
            {
                var details = await _repo.GetByWorkerIdAsync(workerId);
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(MapToResponse) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = false, Message = "Error", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetByBedIdAsync(Guid bedId)
        {
            try
            {
                var details = await _repo.GetByBedIdAsync(bedId);
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(MapToResponse) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = false, Message = "Error", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetByTaskIdAsync(Guid taskId)
        {
            try
            {
                var details = await _repo.GetByTaskIdAsync(taskId);
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(MapToResponse) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = false, Message = "Error", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateAsync(TaskDetailRequest request)
        {
            try
            {
                var entity = new TaskDetail
                {
                    TaskDetailId = Guid.NewGuid(),
                    TaskId = request.TaskId,
                    SeasonId = request.SeasonId,
                    AssignedToWorkerId = request.AssignedToWorkerId,
                    BedId = request.BedId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Notes = request.Notes
                };

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Task detail created" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating task detail", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(Guid id, TaskDetailRequest request)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Task detail not found" };

                entity.TaskId = request.TaskId ?? entity.TaskId;
                entity.SeasonId = request.SeasonId ?? entity.SeasonId;
                entity.AssignedToWorkerId = request.AssignedToWorkerId ?? entity.AssignedToWorkerId;
                entity.BedId = request.BedId ?? entity.BedId;
                entity.StartDate = request.StartDate ?? entity.StartDate;
                entity.EndDate = request.EndDate ?? entity.EndDate;
                entity.Notes = request.Notes ?? entity.Notes;

                _repo.Update(entity);
                await _repo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Task detail updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating task detail", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Task detail not found" };

                _repo.Delete(entity);
                await _repo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Task detail deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting task detail", Errors = new List<string> { ex.Message } };
            }
        }

        private static TaskDetailResponse MapToResponse(TaskDetail d) => new TaskDetailResponse
        {
            TaskDetailId = d.TaskDetailId,
            TaskId = d.TaskId,
            TaskTitle = d.Task?.TaskTitle,
            SeasonId = d.SeasonId,
            AssignedToWorkerId = d.AssignedToWorkerId,
            WorkerName = d.AssignedToWorker?.Fullname,
            BedId = d.BedId,
            BedName = d.Bed?.BedName,
            StartDate = d.StartDate,
            EndDate = d.EndDate,
            Notes = d.Notes
        };
    }
}
