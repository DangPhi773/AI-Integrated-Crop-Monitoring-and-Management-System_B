using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ISubTaskService
    {
        Task<ApiResponse<IEnumerable<SubTaskResponse>>> GetSubTasksByTaskAsync(Guid taskDetailId);
        Task<ApiResponse<SubTaskResponse>> CreateSubTaskAsync(SubTaskCreateRequest request);
        Task<ApiResponse<string>> UpdateSubTaskAsync(Guid id, SubTaskUpdateRequest request);
        Task<ApiResponse<string>> DeleteSubTaskAsync(Guid id);
    }
}
