using CMMS.DAL.DTOs.Attachments;
using CMMS.DAL.DTOs.Auth;
using Microsoft.AspNetCore.Http;

namespace CMMS.BLL.Interfaces
{
    public interface IAttachmentService
    {
        Task<ApiResponse<AttachmentDto>> UploadAsync(IFormFile file, string objectType, Guid objectId, string attachmentType, Guid uploadedBy, string? description = null);
        Task<ApiResponse<List<AttachmentDto>>> GetByObjectAsync(string objectType, Guid objectId);
        Task<ApiResponse<string>> SoftDeleteAsync(Guid attachmentId);
    }
}
