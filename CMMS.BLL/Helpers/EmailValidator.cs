using System.ComponentModel.DataAnnotations;

namespace CMMS.BLL.Helpers
{
    public static class EmailValidator
    {
        private static readonly EmailAddressAttribute _attr = new();

        public static bool IsValid(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            if (email.Length > 255) return false;
            return _attr.IsValid(email);
        }
    }
}
