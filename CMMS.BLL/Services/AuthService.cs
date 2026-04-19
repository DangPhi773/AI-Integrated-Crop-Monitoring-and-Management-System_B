using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CMMS.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IEmailService emailService, IConfiguration config)
        {
            _userRepo = userRepo;
            _emailService = emailService;
            _config = config;
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var exist = await _userRepo.GetByEmailAsync(request.Email);
                if (exist != null) return new ApiResponse<string> { Success = false, Message = "Email đã tồn tại!" };

                var validRoles = new[] { "Worker", "Specialist" };
                string target = string.IsNullOrEmpty(request.TargetRole) ? "Worker" : request.TargetRole;

                if (!validRoles.Contains(target))
                    return new ApiResponse<string> { Success = false, Message = "Vị trí mong muốn không hợp lệ!" };

                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = request.Email,
                    HashPassword = PasswordHelper.HashPassword(request.Password),
                    Fullname = request.Fullname,
                    PhoneNumber = request.PhoneNumber,
                    CreatedAt = DateTimeHelper.VnNow(),
                    Status = "ACTIVE",
                    RequestedRole = target
                };

                await _userRepo.AddAsync(newUser);
                if (await _userRepo.SaveChangesAsync())
                {
                    _ = _emailService.SendEmailAsync(newUser.Email, "Chào mừng",
                $"Bạn đã đăng ký thành công với nguyện vọng vị trí: {target}. Vui lòng đợi hệ thống phê duyệt.");

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

                var jwtSettings = _config.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];
                var key = Encoding.UTF8.GetBytes(secretKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User")
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Issuer = jwtSettings["Issuer"],
                    Audience = jwtSettings["Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Đăng nhập thành công",
                    Data = new { Token = tokenString }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object> { Success = false, Message = "Lỗi hệ thống", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<object>> GetRolesAsync()
        {
            var roles = await _userRepo.GetAllRolesAsync();

            var result = roles.Select(r => new
            {
                r.RoleId,
                r.RoleName
            });

            return new ApiResponse<object>
            {
                Success = true,
                Data = result
            };
        }
    }
}
