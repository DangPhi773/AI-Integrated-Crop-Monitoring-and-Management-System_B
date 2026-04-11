using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Attachments;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepo;
        private readonly ICloudinaryService _cloudinary;

        private static readonly HashSet<string> AllowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg", "image/png", "image/gif", "image/webp",
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };

        public AttachmentService(IAttachmentRepository attachmentRepo, ICloudinaryService cloudinary)
        {
            _attachmentRepo = attachmentRepo;
            _cloudinary = cloudinary;
        }

        public async Task<ApiResponse<AttachmentDto>> UploadAsync(IFormFile file, string objectType, Guid objectId, string attachmentType, Guid uploadedBy, string? description = null)
        {
            if (file == null || file.Length == 0)
                return new ApiResponse<AttachmentDto> { Success = false, Message = "File không hợp lệ" };

            if (!AllowedMimeTypes.Contains(file.ContentType))
                return new ApiResponse<AttachmentDto> { Success = false, Message = $"Loại file '{file.ContentType}' không được phép" };

            long maxSize = file.ContentType.StartsWith("image/") ? 10 * 1024 * 1024 : 25 * 1024 * 1024;
            if (file.Length > maxSize)
                return new ApiResponse<AttachmentDto> { Success = false, Message = $"File vượt quá {maxSize / (1024 * 1024)}MB" };

            var folder = objectType.ToLower() switch
            {
                "report" => "smart-farm/reports",
                "diagnosis_result" => "smart-farm/diagnosis",
                _ => "smart-farm/documents"
            };

            var uploadResult = await _cloudinary.UploadFileAsync(file, folder);

            var entity = new Attachment
            {
                Id = Guid.NewGuid(),
                ObjectType = objectType,
                ObjectId = objectId,
                AttachmentType = attachmentType,
                FileName = uploadResult.FileName,
                FileUrl = uploadResult.Url,
                CloudinaryPublicId = uploadResult.PublicId,
                CloudinarySecureUrl = uploadResult.SecureUrl,
                FileExtension = uploadResult.FileExtension,
                MimeType = uploadResult.MimeType,
                FileSize = uploadResult.FileSize,
                UploadedBy = uploadedBy,
                Description = description,
                IsDeleted = false,
                CreatedAt = DateTimeHelper.VnNow()
            };

            await _attachmentRepo.AddAsync(entity);
            await _attachmentRepo.SaveChangesAsync();

            return new ApiResponse<AttachmentDto>
            {
                Success = true,
                Data = AttachmentMapper.ToDto(entity)
            };
        }

        public async Task<ApiResponse<List<AttachmentDto>>> GetByObjectAsync(string objectType, Guid objectId)
        {
            var items = await _attachmentRepo.GetByObjectAsync(objectType, objectId);
            var data = items.Select(AttachmentMapper.ToDto).ToList();
            return new ApiResponse<List<AttachmentDto>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<string>> SoftDeleteAsync(Guid attachmentId)
        {
            var entity = await _attachmentRepo.GetByIdAsync(attachmentId);
            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };

            entity.IsDeleted = true;

            if (!string.IsNullOrEmpty(entity.CloudinaryPublicId))
                await _cloudinary.DeleteFileAsync(entity.CloudinaryPublicId);

            await _attachmentRepo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Đã xóa" };
        }

    }
}
