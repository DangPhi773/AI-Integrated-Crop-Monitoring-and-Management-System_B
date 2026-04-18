using System.Security.Cryptography;
using System.Text;

namespace CMMS.BLL.Helpers
{
    public static class DeviceApiKeyHelper
    {
        private const string Prefix = "iot_";
        private const int RandomByteLength = 32;

        public static string GenerateKey()
        {
            var bytes = RandomNumberGenerator.GetBytes(RandomByteLength);
            var encoded = Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
            return Prefix + encoded;
        }

        public static string HashKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be empty.", nameof(key));

            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(key.Trim()));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
