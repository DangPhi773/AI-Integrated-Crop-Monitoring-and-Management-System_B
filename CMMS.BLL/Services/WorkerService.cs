using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Users;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CMMS.BLL.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepo;
        private readonly IServiceScopeFactory _scopeFactory;

        public WorkerService(IWorkerRepository workerRepo, IServiceScopeFactory scopeFactory)
        {
            _workerRepo = workerRepo;
            _scopeFactory = scopeFactory;
        }

        public async System.Threading.Tasks.Task<ApiResponse<IEnumerable<UserResponse>>> GetListOfWorkersAsync()
        {
            try
            {
                var workers = await _workerRepo.GetAllWorkersAsync();
                var data = workers.Select(w => new UserResponse
                {
                    UserId = w.UserId,
                    Email = w.Email,
                    Fullname = w.Fullname,
                    PhoneNumber = w.PhoneNumber,
                    Status = w.Status,
                    CreatedAt = w.CreatedAt,
                    RoleName = w.Role?.RoleName
                });

                return new ApiResponse<IEnumerable<UserResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<UserResponse>> { Success = false, Message = "Lỗi khi lấy danh sách Worker", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<UserResponse>> GetWorkerDetailAsync(Guid id)
        {
            try
            {
                var w = await _workerRepo.GetWorkerByIdAsync(id);
                if (w == null) return new ApiResponse<UserResponse> { Success = false, Message = "Không tìm thấy Worker này." };

                var data = new UserResponse
                {
                    UserId = w.UserId,
                    Email = w.Email,
                    Fullname = w.Fullname,
                    PhoneNumber = w.PhoneNumber,
                    Status = w.Status,
                    CreatedAt = w.CreatedAt,
                    RoleName = w.Role?.RoleName
                };

                return new ApiResponse<UserResponse> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserResponse> { Success = false, Message = "Lỗi hệ thống", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> CreateNewWorkerAsync(WorkerRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return new ApiResponse<string> { Success = false, Message = "Email không được để trống." };
                }

                if (await _workerRepo.EmailExistsAsync(request.Email))
                {
                    return new ApiResponse<string> { Success = false, Message = $"Email '{request.Email}' đã tồn tại trong hệ thống." };
                }

                var workerRole = await _workerRepo.GetRoleByNameAsync("Worker");

                if (workerRole == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Hệ thống chưa cấu hình Role 'Worker' trong DB."
                    };
                }


                var newWorker = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = request.Email,
                    Password = request.Password,
                    HashPassword = PasswordHelper.HashPassword(request.Password),
                    Fullname = request.Fullname,
                    PhoneNumber = request.PhoneNumber,
                    RoleId = workerRole.RoleId,
                    Status = "Active",
                    CreatedAt = DateTimeHelper.VnNow()
                };

                await _workerRepo.AddWorkerAsync(newWorker);
                await _workerRepo.SaveChangesAsync();

                var workerId = newWorker.UserId;
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var notify = scope.ServiceProvider.GetRequiredService<INotificationService>();
                        await notify.NotifyNewWorkerAsync(workerId);
                    }
                    catch { }
                });

                return new ApiResponse<string> { Success = true, Message = "Tạo Worker thành công." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Lỗi khi tạo Worker",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> UpdateWorkerInfoAsync(Guid id, WorkerRequest request)
        {
            try
            {
                var worker = await _workerRepo.GetWorkerByIdAsync(id);
                if (worker == null) return new ApiResponse<string> { Success = false, Message = "Worker không tồn tại." };

                worker.Fullname = request.Fullname;
                worker.PhoneNumber = request.PhoneNumber;
                worker.Status = request.Status;
                if (request.RoleId.HasValue) worker.RoleId = request.RoleId;

                _workerRepo.UpdateWorker(worker);
                await _workerRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Cập nhật thông tin thành công." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi cập nhật", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> ChangeWorkerStatusAsync(Guid id, string status)
        {
            try
            {
                var worker = await _workerRepo.GetWorkerByIdAsync(id);
                if (worker == null) return new ApiResponse<string> { Success = false, Message = "Worker không tồn tại." };

                worker.Status = status;
                _workerRepo.UpdateWorker(worker);
                await _workerRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = $"Đã chuyển trạng thái sang: {status}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi thay đổi trạng thái", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> RemoveWorkerAsync(Guid id)
        {
            try
            {
                var worker = await _workerRepo.GetWorkerByIdAsync(id);
                if (worker == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy Worker để xóa." };

                _workerRepo.DeleteWorker(worker);
                await _workerRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Đã xóa tài khoản Worker thành công." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi khi xóa", Errors = new List<string> { ex.Message } };
            }
        }
    }
}