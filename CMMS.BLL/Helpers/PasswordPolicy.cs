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
            "password", "password1", "password123", "12345678", "123456789", "1234567890",
            "qwerty", "qwerty123", "abc12345", "admin", "admin123", "letmein", "welcome",
            "welcome1", "iloveyou", "monkey", "dragon", "sunshine", "princess", "matkhau",
            "matkhau123"
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

            if (password.Any(char.IsWhiteSpace))
                errors.Add("Mật khẩu không được chứa khoảng trắng.");

            if (CommonPasswords.Contains(password))
                errors.Add("Mật khẩu quá phổ biến, vui lòng chọn mật khẩu khác.");

            return errors;
        }

        public static bool IsValid(string? password) => Validate(password).Count == 0;
    }
}
