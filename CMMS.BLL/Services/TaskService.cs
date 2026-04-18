using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Interfaces;
using CMMS.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var data = tasks.Select(TaskMapper.ToResponse);
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
                var t = await _taskRepo.GetByIdAsync(id);
                if (t == null) return new ApiResponse<TaskResponse> { Success = false, Message = "Task not found" };

                return new ApiResponse<TaskResponse>
                {
                    Success = true,
                    Data = TaskMapper.ToResponse(t)
                };
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
                var entity = TaskMapper.ToEntity(request);

                await _taskRepo.AddAsync(entity);
                await _taskRepo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Task created" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating task", Errors = new List<string> { ex.Message } };
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
                entity.TaskType = request.TaskType ?? entity.TaskType; 

                _taskRepo.Update(entity);
                await _taskRepo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Task updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating task", Errors = new List<string> { ex.Message } };
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
    }
}
