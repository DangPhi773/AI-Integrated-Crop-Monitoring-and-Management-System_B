using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.BLL.Realtime;
using CMMS.BLL.Helpers;
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
        private readonly ITaskRealtime _realtime;

        public TaskDetailService(
            ITaskDetailRepository repo,
            IWorkerScheduleRepository scheduleRepo,
            IUserRepository userRepo,
            ITaskRealtime realtime)
        {
            _repo = repo;
            _scheduleRepo = scheduleRepo;
            _userRepo = userRepo;
            _realtime = realtime;
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

                var pastResult = ValidateNotPast(request.StartDate);
                if (pastResult != null) return pastResult;

                var scheduleResult = await ValidateScheduleAsync(null, workerIds, request.StartDate, request.EndDate);
                if (scheduleResult != null) return scheduleResult;

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
                    Status = request.Status ?? (workerIds.Count > 0 ? "Assigned" : "Pending")
                };

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync();

                await SyncWorkerSchedulesAsync(entity.TaskDetailId, workerIds, entity.Task?.TaskTitle);

                await PushTaskDetailEventsAsync(entity, workerIds, "created");

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

                var oldWorkerIds = entity.AssignedToWorkerIds?.ToList() ?? new List<Guid>();

                var newWorkerIds = request.AssignedToWorkerIds;
                if (newWorkerIds != null)
                {
                    var validationResult = await ValidateWorkersAsync(newWorkerIds);
                    if (validationResult != null) return validationResult;
                }

                var pastResult = ValidateNotPast(request.StartDate);
                if (pastResult != null) return pastResult;

                var effectiveWorkerIds = newWorkerIds ?? oldWorkerIds;
                var effectiveStart = request.StartDate ?? entity.StartDate;
                var effectiveEnd = request.EndDate ?? entity.EndDate;
                var scheduleResult = await ValidateScheduleAsync(entity.TaskDetailId, effectiveWorkerIds, effectiveStart, effectiveEnd);
                if (scheduleResult != null) return scheduleResult;

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

                if (entity.Status == "Pending" && (entity.AssignedToWorkerIds?.Count ?? 0) > 0)
                    entity.Status = "Assigned";

                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                if (newWorkerIds != null)
                {
                    await SyncWorkerSchedulesAsync(entity.TaskDetailId, newWorkerIds, entity.Task?.TaskTitle);
                }

                var affectedWorkerIds = oldWorkerIds.Union(entity.AssignedToWorkerIds ?? new List<Guid>()).ToList();
                await PushTaskDetailEventsAsync(entity, affectedWorkerIds, "updated");

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
                var validStatuses = new[] { "Pending", "Assigned", "InProgress", "Completed", "Failed", "Cancelled" };
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

                await PushTaskDetailEventsAsync(entity, entity.AssignedToWorkerIds, "status-changed");

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

                var affectedWorkerIds = entity.AssignedToWorkerIds?.ToList() ?? new List<Guid>();
                var farmId = entity.FarmId;
                var taskDetailId = entity.TaskDetailId;

                var existingSchedules = await _scheduleRepo.GetByTaskDetailIdTrackingAsync(id);
                if (existingSchedules.Any())
                    _scheduleRepo.DeleteRange(existingSchedules);

                _repo.Delete(entity);
                await _repo.SaveChangesAsync();

                if (farmId.HasValue)
                {
                    await _realtime.PushTaskUpdatedAsync(farmId.Value, new
                    {
                        action = "deleted",
                        taskDetailId
                    });
                }
                foreach (var workerId in affectedWorkerIds)
                {
                    await _realtime.PushScheduleUpdatedAsync(workerId, new
                    {
                        action = "deleted",
                        taskDetailId
                    });
                }

                return new ApiResponse<string> { Success = true, Message = "Task detail deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting task detail", Errors = new List<string> { ex.Message } };
            }
        }

        private async System.Threading.Tasks.Task PushTaskDetailEventsAsync(TaskDetail entity, IEnumerable<Guid> workerIds, string action)
        {
            if (entity.FarmId.HasValue)
            {
                await _realtime.PushTaskUpdatedAsync(entity.FarmId.Value, new
                {
                    action,
                    taskDetailId = entity.TaskDetailId,
                    taskId = entity.TaskId,
                    seasonId = entity.SeasonId,
                    farmId = entity.FarmId,
                    status = entity.Status,
                    startDate = entity.StartDate,
                    endDate = entity.EndDate,
                    assignedToWorkerIds = entity.AssignedToWorkerIds
                });
            }

            foreach (var workerId in workerIds)
            {
                await _realtime.PushScheduleUpdatedAsync(workerId, new
                {
                    action,
                    taskDetailId = entity.TaskDetailId,
                    farmId = entity.FarmId,
                    status = entity.Status,
                    startDate = entity.StartDate,
                    endDate = entity.EndDate
                });
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

        private static ApiResponse<string>? ValidateNotPast(DateTime? start)
        {
            if (start.HasValue && start.Value.Date < DateTimeHelper.VnNow().Date)
                return new ApiResponse<string> { Success = false, Message = "Không thể giao việc trong quá khứ" };
            return null;
        }

        private async Task<ApiResponse<string>?> ValidateScheduleAsync(Guid? selfId, List<Guid> workerIds, DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue && start.Value > end.Value)
                return new ApiResponse<string> { Success = false, Message = "Giờ bắt đầu không được sau giờ kết thúc" };

            if (!start.HasValue || !end.HasValue || workerIds.Count == 0)
                return null;

            var overlapping = await _repo.GetActiveOverlappingAsync(start.Value, end.Value, selfId);
            var conflictWorkerIds = overlapping
                .SelectMany(d => d.AssignedToWorkerIds)
                .Where(workerIds.Contains)
                .Distinct()
                .ToList();

            if (conflictWorkerIds.Count > 0)
            {
                var workers = await _userRepo.GetByRoleNamesAsync("Worker");
                var nameById = workers.ToDictionary(w => w.UserId, w => w.Fullname ?? w.Email ?? w.UserId.ToString());
                var names = conflictWorkerIds.Select(id => nameById.TryGetValue(id, out var n) ? n : id.ToString());
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Worker bị trùng lịch trong khung giờ này: {string.Join(", ", names)}"
                };
            }

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
