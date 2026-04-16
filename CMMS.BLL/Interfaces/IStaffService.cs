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
    public interface IStaffService
    {
        Task<ApiResponse<IEnumerable<UserResponse>>> GetStaffListAsync();
        Task<ApiResponse<UserResponse>> GetStaffDetailAsync(Guid id);
        Task<ApiResponse<string>> AssignRoleAsync(Guid userId, string roleName);
        Task<ApiResponse<string>> CreateStaffAsync(StaffRequest request, string roleName);
        Task<ApiResponse<string>> UpdateStaffAsync(Guid id, StaffRequest request);
        Task<ApiResponse<string>> RemoveStaffAsync(Guid id);
        Task<ApiResponse<IEnumerable<UserResponse>>> GetUsersWithoutRoleAsync();
    }
}
