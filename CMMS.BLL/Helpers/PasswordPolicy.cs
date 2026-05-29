using System.Collections.Generic;
using System.Linq;

namespace CMMS.BLL.Helpers
{
    public static class PasswordPolicy
    {
        public const int MinLength = 8;
        public const int MaxLength = 100;

        private static readonly HashSet<string> CommonPasswords = new(System.StringComparer.OrdinalIgnoreCase)
        {
            "P@ssw0rd", "P@ssw0rd1", "Passw0rd!", "Passw0rd@123", "Admin@123",
            "Admin@1234", "Welcome@1", "Welcome@123", "Qwerty@123", "Iloveyou@1",
            "Abc@12345", "Matkhau@123"
        };

        public static List<string> Validate(string? password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Mật khẩu không được để trống.");
                return errors;
            }

            if (password.Length < MinLength)
                errors.Add($"Mật khẩu phải có ít nhất {MinLength} ký tự.");

            if (password.Length > MaxLength)
                errors.Add($"Mật khẩu tối đa {MaxLength} ký tự.");

            if (!password.Any(char.IsLetter))
                errors.Add("Mật khẩu phải chứa ít nhất 1 chữ cái.");

            if (!password.Any(char.IsDigit))
                errors.Add("Mật khẩu phải chứa ít nhất 1 chữ số.");

            if (!password.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)))
                errors.Add("Mật khẩu phải chứa ít nhất 1 ký tự đặc biệt (ví dụ: !@#$%^&*).");

            if (password.Any(char.IsWhiteSpace))
                errors.Add("Mật khẩu không được chứa khoảng trắng.");

            if (CommonPasswords.Contains(password))
                errors.Add("Mật khẩu quá phổ biến, vui lòng chọn mật khẩu khác.");

            return errors;
        }

        public static bool IsValid(string? password) => Validate(password).Count == 0;
    }
}
