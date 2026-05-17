using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Helpers;

public static class FileValidator
{
    public static (bool Ok, string? Error) ValidateImage(IFormFile? file, int maxMb = 10)
    {
        if (file == null || file.Length == 0)
            return (false, "File không hợp lệ");

        if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            return (false, $"Chỉ chấp nhận file ảnh, không nhận '{file.ContentType}'");

        var maxBytes = (long)maxMb * 1024 * 1024;
        if (file.Length > maxBytes)
            return (false, $"File vượt quá {maxMb}MB");

        return (true, null);
    }
}
