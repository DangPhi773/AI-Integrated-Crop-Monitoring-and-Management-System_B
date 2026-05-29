using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Auth
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại là bắt buộc")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu mới phải từ 8 đến 100 ký tự")]
        public string NewPassword { get; set; } = null!;
    }
}
