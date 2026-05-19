using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(255, ErrorMessage = "Email tối đa 255 ký tự")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Mật khẩu không được rỗng")]
        public string Password { get; set; } = null!;
    }
}
