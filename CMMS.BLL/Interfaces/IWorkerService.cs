using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IWorkerService
    {
        System.Threading.Tasks.Task<ApiResponse<IEnumerable<UserResponse>>> GetListOfWorkersAsync();
        System.Threading.Tasks.Task<ApiResponse<UserResponse>> GetWorkerDetailAsync(Guid id);
        System.Threading.Tasks.Task<ApiResponse<string>> CreateNewWorkerAsync(WorkerRequest request);
        System.Threading.Tasks.Task<ApiResponse<string>> UpdateWorkerInfoAsync(Guid id, WorkerRequest request);
        System.Threading.Tasks.Task<ApiResponse<string>> ChangeWorkerStatusAsync(Guid id, string status);
        System.Threading.Tasks.Task<ApiResponse<string>> RemoveWorkerAsync(Guid id);
    }
}
