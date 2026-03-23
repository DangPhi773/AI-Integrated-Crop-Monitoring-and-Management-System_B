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
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;

        public TaskService(ITaskRepository taskRepo) => _taskRepo = taskRepo;

        public async System.Threading.Tasks.Task<ApiResponse<IEnumerable<TaskResponse>>> GetAllTasksAsync()
        {
            try
            {
                var tasks = await _taskRepo.GetAllAsync();
                var data = tasks.Select(MapToResponse);
                return new ApiResponse<IEnumerable<TaskResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<TaskResponse>> { Success = false, Message = "Error fetching tasks", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<TaskResponse>> GetTaskByIdAsync(Guid id)
        {
            try
            {
                var task = await _taskRepo.GetByIdAsync(id);
                if (task == null) return new ApiResponse<TaskResponse> { Success = false, Message = "Task not found" };
                return new ApiResponse<TaskResponse> { Success = true, Data = MapToResponse(task) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TaskResponse> { Success = false, Message = "Error fetching task", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> CreateTaskAsync(TaskRequest request)
        {
            try
            {
                var entity = new DAL.Entities.Task
                {
                    TaskId = Guid.NewGuid(),
                    TaskTitle = request.TaskTitle,
                    TaskNotes = request.TaskNotes,
                    TaskStatus = request.TaskStatus ?? "Pending",
                    TaskScheduledAt = request.TaskScheduledAt,
                    TaskCreatedAt = DateTime.UtcNow,
                    TaskDetails = request.TaskDetails?.Select(d => new TaskDetail
                    {
                        TaskDetailId = Guid.NewGuid(),
                        SeasonId = d.SeasonId,
                        AssignedToWorkerId = d.AssignedToWorkerId,
                        StartDate = d.StartDate,
                        EndDate = d.EndDate,
                        Notes = d.Notes
                    }).ToList() ?? new List<TaskDetail>()
                };

                await _taskRepo.AddAsync(entity);
                await _taskRepo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Task created with details" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> UpdateTaskAsync(Guid id, TaskRequest request)
        {
            try
            {
                var entity = await _taskRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Task not found" };

                entity.TaskTitle = request.TaskTitle ?? entity.TaskTitle;
                entity.TaskNotes = request.TaskNotes ?? entity.TaskNotes;
                entity.TaskStatus = request.TaskStatus ?? entity.TaskStatus;
                entity.TaskScheduledAt = request.TaskScheduledAt ?? entity.TaskScheduledAt;

                if (request.TaskDetails != null)
                {
                    foreach (var detailReq in request.TaskDetails)
                    {
                        var existingDetail = entity.TaskDetails
                            .FirstOrDefault(d => d.TaskDetailId == detailReq.TaskDetailId);

                        if (existingDetail != null)
                        {
                            existingDetail.SeasonId = detailReq.SeasonId ?? existingDetail.SeasonId;
                            existingDetail.AssignedToWorkerId = detailReq.AssignedToWorkerId ?? existingDetail.AssignedToWorkerId;
                            existingDetail.StartDate = detailReq.StartDate ?? existingDetail.StartDate;
                            existingDetail.EndDate = detailReq.EndDate ?? existingDetail.EndDate;
                            existingDetail.Notes = detailReq.Notes ?? existingDetail.Notes;
                        }
                    }
                }

                _taskRepo.Update(entity);
                await _taskRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Task updated successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Update failed", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> DeleteTaskAsync(Guid id)
        {
            try
            {
                var entity = await _taskRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Task not found" };

                _taskRepo.Delete(entity);
                await _taskRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Task deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting task", Errors = new List<string> { ex.Message } };
            }
        }

        // --- helpers ---
        private static TaskResponse MapToResponse(DAL.Entities.Task t) =>
        new TaskResponse
        {
            TaskId = t.TaskId,
            TaskTitle = t.TaskTitle,
            TaskNotes = t.TaskNotes,
            TaskStatus = t.TaskStatus,
            TaskScheduledAt = t.TaskScheduledAt,
            TaskCreatedAt = t.TaskCreatedAt,
            TaskDetails = t.TaskDetails?.Select(d => new TaskDetailDto
            {
                TaskDetailId = d.TaskDetailId,
                SeasonId = d.SeasonId,
                AssignedToWorkerId = d.AssignedToWorkerId,
                WorkerName = d.AssignedToWorker?.Fullname, 
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                Notes = d.Notes
            }).ToList() ?? new List<TaskDetailDto>()
        };

    }
}
