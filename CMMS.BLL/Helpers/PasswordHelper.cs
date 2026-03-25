using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Băm mật khẩu thô thành chuỗi Hash an toàn.
        /// </summary>
        /// <param name="password">Mật khẩu người dùng nhập vào</param>
        /// <returns>Chuỗi mật khẩu đã được băm kèm Salt</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return string.Empty;
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Kiểm tra mật khẩu thô có khớp với chuỗi đã băm trong Database không.
        /// </summary>
        /// <param name="password">Mật khẩu thô từ màn hình Login</param>
        /// <param name="hashedPassword">Chuỗi HashPassword lấy từ DB</param>
        /// <returns>True nếu khớp, False nếu sai</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
}
