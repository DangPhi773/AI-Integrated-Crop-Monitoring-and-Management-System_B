using CMMS.DAL.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IAuthService
    {
        System.Threading.Tasks.Task<ApiResponse<string>> RegisterAsync(RegisterRequest request);
        System.Threading.Tasks.Task<ApiResponse<object>> LoginAsync(LoginRequest request);
        Task<ApiResponse<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
        Task<ApiResponse<object>> GetRolesAsync();
    }
}
