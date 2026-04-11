using CMMS.DAL.DTOs.CloudStorage;
using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Interfaces
{
    public interface ICloudinaryService
    {
        Task<CloudinaryUploadResult> UploadFileAsync(IFormFile file, string folder = "smart-farm");
        Task<bool> DeleteFileAsync(string publicId);
    }
}
