using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
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
        private readonly IWorkerScheduleRepository _scheduleRepo;
        private readonly IUserRepository _userRepo;

        public TaskDetailService(
            ITaskDetailRepository repo,
            IWorkerScheduleRepository scheduleRepo,
            IUserRepository userRepo)
        {
            _repo = repo;
            _scheduleRepo = scheduleRepo;
            _userRepo = userRepo;
        }

        public async Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetAllAsync()
        {
            try
            {
                var details = await _repo.GetAllAsync();
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(TaskDetailMapper.ToResponse) };
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
                return new ApiResponse<TaskDetailResponse> { Success = true, Data = TaskDetailMapper.ToResponse(d) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TaskDetailResponse> { Success = false, Message = "Error", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetByTaskIdAsync(Guid taskId)
        {
            try
            {
                var details = await _repo.GetByTaskIdAsync(taskId);
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(TaskDetailMapper.ToResponse) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = false, Message = "Error", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<TaskDetailResponse>>> GetBySeasonIdAsync(Guid seasonId)
        {
            try
            {
                var details = await _repo.GetBySeasonIdAsync(seasonId);
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(TaskDetailMapper.ToResponse) };
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
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(TaskDetailMapper.ToResponse) };
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
                return new ApiResponse<IEnumerable<TaskDetailResponse>> { Success = true, Data = details.Select(TaskDetailMapper.ToResponse) };
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
                var workerIds = request.AssignedToWorkerIds ?? new List<Guid>();

                var validationResult = await ValidateWorkersAsync(workerIds);
                if (validationResult != null) return validationResult;

                var entity = new TaskDetail
                {
                    TaskDetailId = Guid.NewGuid(),
                    TaskId = request.TaskId,
                    SeasonId = request.SeasonId,
                    FarmId = request.FarmId,
                    AssignedToWorkerIds = workerIds,
                    BedIds = request.BedIds ?? new List<Guid>(),
                    PlotIds = request.PlotIds ?? new List<Guid>(),
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Notes = request.Notes,
                    Status = request.Status ?? "Pending"
                };

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync();

                await SyncWorkerSchedulesAsync(entity.TaskDetailId, workerIds, entity.Task?.TaskTitle);

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

                var newWorkerIds = request.AssignedToWorkerIds;
                if (newWorkerIds != null)
                {
                    var validationResult = await ValidateWorkersAsync(newWorkerIds);
                    if (validationResult != null) return validationResult;
                }

                entity.TaskId = request.TaskId ?? entity.TaskId;
                entity.SeasonId = request.SeasonId ?? entity.SeasonId;
                entity.FarmId = request.FarmId ?? entity.FarmId;
                entity.AssignedToWorkerIds = newWorkerIds ?? entity.AssignedToWorkerIds;
                entity.BedIds = request.BedIds ?? entity.BedIds;
                entity.PlotIds = request.PlotIds ?? entity.PlotIds;
                entity.StartDate = request.StartDate ?? entity.StartDate;
                entity.EndDate = request.EndDate ?? entity.EndDate;
                entity.Notes = request.Notes ?? entity.Notes;
                entity.Status = request.Status ?? entity.Status;

                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                if (newWorkerIds != null)
                {
                    await SyncWorkerSchedulesAsync(entity.TaskDetailId, newWorkerIds, entity.Task?.TaskTitle);
                }

                return new ApiResponse<string> { Success = true, Message = "Task detail updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating task detail", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateStatusAsync(Guid id, string status, Guid workerId)
        {
            try
            {
                var validStatuses = new[] { "Pending", "InProgress", "Completed", "Cancelled" };
                if (!validStatuses.Contains(status))
                    return new ApiResponse<string> { Success = false, Message = $"Trạng thái không hợp lệ. Chỉ chấp nhận: {string.Join(", ", validStatuses)}" };

                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<string> { Success = false, Message = "Không tìm thấy task detail" };

                if (!entity.AssignedToWorkerIds.Contains(workerId))
                    return new ApiResponse<string> { Success = false, Message = "Bạn không được phân công cho công việc này" };

                entity.Status = status;
                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = $"Đã cập nhật trạng thái thành '{status}'" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi cập nhật trạng thái", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Task detail not found" };

                var existingSchedules = await _scheduleRepo.GetByTaskDetailIdTrackingAsync(id);
                if (existingSchedules.Any())
                    _scheduleRepo.DeleteRange(existingSchedules);

                _repo.Delete(entity);
                await _repo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Task detail deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting task detail", Errors = new List<string> { ex.Message } };
            }
        }

        private async Task<ApiResponse<string>?> ValidateWorkersAsync(List<Guid> workerIds)
        {
            if (!workerIds.Any()) return null;

            var workers = await _userRepo.GetByRoleNamesAsync("Worker");
            var validWorkerIds = workers.Select(w => w.UserId).ToHashSet();
            var invalidIds = workerIds.Where(id => !validWorkerIds.Contains(id)).ToList();

            if (invalidIds.Any())
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Worker không hợp lệ: {string.Join(", ", invalidIds)}"
                };

            return null;
        }

        private async System.Threading.Tasks.Task SyncWorkerSchedulesAsync(Guid taskDetailId, List<Guid> workerIds, string? taskTitle)
        {
            var existingSchedules = await _scheduleRepo.GetByTaskDetailIdTrackingAsync(taskDetailId);
            var existingWorkerIds = existingSchedules.Select(s => s.WorkerId!.Value).ToHashSet();
            var newWorkerIdSet = workerIds.ToHashSet();

            var toRemove = existingSchedules.Where(s => !newWorkerIdSet.Contains(s.WorkerId!.Value)).ToList();
            if (toRemove.Any())
                _scheduleRepo.DeleteRange(toRemove);

            var toAdd = workerIds.Where(id => !existingWorkerIds.Contains(id)).ToList();
            if (toAdd.Any())
            {
                var newSchedules = toAdd.Select(wId => new WorkerSchedule
                {
                    ScheduleId = Guid.NewGuid(),
                    TaskDetailId = taskDetailId,
                    WorkerId = wId,
                    Description = taskTitle,
                    Status = "Assigned"
                });
                await _scheduleRepo.AddRangeAsync(newSchedules);
            }

            if (toRemove.Any() || toAdd.Any())
                await _scheduleRepo.SaveChangesAsync();
        }
    }
}
