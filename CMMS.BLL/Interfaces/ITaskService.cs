using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<ApiResponse<IEnumerable<TaskResponse>>> GetAllTasksAsync();
        System.Threading.Tasks.Task<ApiResponse<TaskResponse>> GetTaskByIdAsync(Guid id);
        System.Threading.Tasks.Task<ApiResponse<string>> CreateTaskAsync(TaskRequest request);
        System.Threading.Tasks.Task<ApiResponse<string>> UpdateTaskAsync(Guid id, TaskRequest request);
        System.Threading.Tasks.Task<ApiResponse<string>> DeleteTaskAsync(Guid id);
    }
}
