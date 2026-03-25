using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using CMMS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var exist = await _userRepo.GetByEmailAsync(request.Email);
                if (exist != null) return new ApiResponse<string> { Success = false, Message = "Email đã tồn tại!" };

                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = request.Email,
                    Password = request.Password,
                    HashPassword = PasswordHelper.HashPassword(request.Password), 

                    Fullname = request.Fullname,
                    PhoneNumber = request.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active"
                };

                await _userRepo.AddAsync(newUser);
                if (await _userRepo.SaveChangesAsync())
                {
                    _ = SendEmailAsync(newUser.Email, "Chào mừng", "Bạn đã đăng ký thành công hệ thống CMMS.");

                    return new ApiResponse<string> { Success = true, Message = "Đăng ký thành công!" };
                }
                return new ApiResponse<string> { Success = false, Message = "Lỗi lưu dữ liệu." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi BLL", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<object>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepo.GetByEmailAsync(request.Email);
                if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.HashPassword))
                {
                    return new ApiResponse<object> { Success = false, Message = "Thông tin đăng nhập không chính xác." };
                }

                return new ApiResponse<object>
                {
                    Success = true,
                    Data = new { user.UserId, user.Email, Role = user.Role?.RoleName }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object> { Success = false, Message = "Lỗi hệ thống", Errors = new List<string> { ex.Message } };
            }
        }

        private async System.Threading.Tasks.Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var fromMail = "your-email@gmail.com";
                var pw = "your-app-password";
                using var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromMail, pw)
                };
                await client.SendMailAsync(new MailMessage(fromMail, toEmail, subject, body));
            }
            catch {  }
        }
    }
}
