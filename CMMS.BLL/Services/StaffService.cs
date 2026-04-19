using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Users;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CMMS.BLL.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepo;
        private readonly IServiceScopeFactory _scopeFactory;

        public StaffService(IStaffRepository staffRepo, IServiceScopeFactory scopeFactory)
        {
            _staffRepo = staffRepo;
            _scopeFactory = scopeFactory;
        }

        public async Task<ApiResponse<IEnumerable<UserResponse>>> GetStaffListAsync()
        {
            try
            {
                var staffs = await _staffRepo.GetAllStaffsAsync();
                var activeStaffs = staffs.Where(u => u.Status != null && u.Status.ToUpper() == "ACTIVE");
                var data = activeStaffs.Select(UserMapper.ToResponse);
                return new ApiResponse<IEnumerable<UserResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<UserResponse>> { Success = false, Message = "Lỗi lấy danh sách nhân sự", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<UserResponse>> GetStaffDetailAsync(Guid id)
        {
            var user = await _staffRepo.GetUserByIdAsync(id);
            if (user == null || user.Status != "ACTIVE") return new ApiResponse<UserResponse> { Success = false, Message = "Không tìm thấy người dùng." };

            return new ApiResponse<UserResponse>
            {
                Success = true,
                Data = UserMapper.ToResponse(user)
            };
        }

        public async Task<ApiResponse<string>> AssignRoleAsync(Guid userId, string roleName)
        {
            try
            {
                var user = await _staffRepo.GetUserByIdAsync(userId);
                if (user == null) return new ApiResponse<string> { Success = false, Message = "User không tồn tại." };

                var role = await _staffRepo.GetRoleByNameAsync(roleName);
                if (role == null) return new ApiResponse<string> { Success = false, Message = $"Role '{roleName}' không tồn tại trong hệ thống." };

                user.RoleId = role.RoleId;
                _staffRepo.UpdateUser(user);
                await _staffRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = $"Đã cấp quyền '{roleName}' cho tài khoản {user.Email}." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi khi gán quyền", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateStaffAsync(StaffRequest request, string roleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                    return new ApiResponse<string> { Success = false, Message = "Email không được để trống." };

                if (await _staffRepo.EmailExistsAsync(request.Email))
                    return new ApiResponse<string> { Success = false, Message = $"Email '{request.Email}' đã tồn tại trong hệ thống." };

                var role = await _staffRepo.GetRoleByNameAsync(roleName);
                if (role == null) return new ApiResponse<string> { Success = false, Message = "Role không hợp lệ." };

                var newUser = UserMapper.ToEntity(request);
                newUser.UserId = Guid.NewGuid();
                newUser.RoleId = role.RoleId;
                newUser.CreatedAt = DateTimeHelper.VnNow();
                newUser.Status = "ACTIVE";

                await _staffRepo.AddUserAsync(newUser);
                await _staffRepo.SaveChangesAsync();

                var userId = newUser.UserId;
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var notify = scope.ServiceProvider.GetRequiredService<INotificationService>();
                        await notify.NotifyNewWorkerAsync(userId);
                    }
                    catch { }
                });

                return new ApiResponse<string> { Success = true, Message = $"Tạo {roleName} thành công." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi tạo nhân sự", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateStaffAsync(Guid id, StaffRequest request)
        {
            var user = await _staffRepo.GetUserByIdAsync(id);
            if (user == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy nhân sự." };

            user.Fullname = request.Fullname;
            user.PhoneNumber = request.PhoneNumber;
            user.Status = request.Status;
            if (request.RoleId.HasValue) user.RoleId = request.RoleId;

            _staffRepo.UpdateUser(user);
            await _staffRepo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Cập nhật thành công." };
        }

        public async Task<ApiResponse<string>> RemoveStaffAsync(Guid id)
        {
            var user = await _staffRepo.GetUserByIdAsync(id);
            if (user == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy để xóa." };

            if (user.Status == "INACTIVE")
                return new ApiResponse<string> { Success = false, Message = "Nhân viên này đã bị xóa trước đó." };

            user.Status = "INACTIVE";
            _staffRepo.UpdateUser(user);
            await _staffRepo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Xóa mềm thành công." };
        }

        public async Task<ApiResponse<IEnumerable<UserResponse>>> GetUsersWithoutRoleAsync()
        {
            try
            {
                var users = await _staffRepo.GetUsersWithoutRoleAsync();
                var data = users.Select(UserMapper.ToResponse);

                return new ApiResponse<IEnumerable<UserResponse>>
                {
                    Success = true,
                    Data = data,
                    Message = data.Any() ? "Lấy danh sách thành công" : "Không có user nào chưa phân role"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<UserResponse>>
                {
                    Success = false,
                    Message = "Lỗi lấy danh sách user chưa có role",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<UserResponse>> GetMyProfileAsync(Guid userId)
        {
            try
            {
                var user = await _staffRepo.GetProfileByIdAsync(userId);

                if (user == null)
                    return new ApiResponse<UserResponse> { Success = false, Message = "Profile not found" };

                return new ApiResponse<UserResponse>
                {
                    Success = true,
                    Data = UserMapper.ToResponse(user)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserResponse> { Success = false, Message = ex.Message };
            }
        }
    }
}