using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.CloudStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CMMS.BLL.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;
            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<CloudinaryUploadResult> UploadFileAsync(IFormFile file, string folder = "smart-farm")
        {
            await using var stream = file.OpenReadStream();
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var ext = Path.GetExtension(file.FileName);

            var isImage = file.ContentType.StartsWith("image/");

            if (isImage)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    PublicId = $"{fileName}_{Guid.NewGuid():N}"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                return new CloudinaryUploadResult
                {
                    PublicId = result.PublicId,
                    Url = result.Url?.ToString() ?? string.Empty,
                    SecureUrl = result.SecureUrl?.ToString() ?? string.Empty,
                    FileName = file.FileName,
                    FileExtension = ext,
                    MimeType = file.ContentType,
                    FileSize = file.Length
                };
            }
            else
            {
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    PublicId = $"{fileName}_{Guid.NewGuid():N}"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                return new CloudinaryUploadResult
                {
                    PublicId = result.PublicId,
                    Url = result.Url?.ToString() ?? string.Empty,
                    SecureUrl = result.SecureUrl?.ToString() ?? string.Empty,
                    FileName = file.FileName,
                    FileExtension = ext,
                    MimeType = file.ContentType,
                    FileSize = file.Length
                };
            }
        }

        public async Task<bool> DeleteFileAsync(string publicId)
        {
            var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
            return result.Result == "ok";
        }
    }
}
