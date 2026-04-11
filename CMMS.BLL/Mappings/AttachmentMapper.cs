using CMMS.DAL.DTOs.Attachments;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class AttachmentMapper
    {
        public static AttachmentDto ToDto(Attachment a) => new()
        {
            Id = a.Id,
            ObjectType = a.ObjectType,
            ObjectId = a.ObjectId,
            AttachmentType = a.AttachmentType,
            FileName = a.FileName,
            FileUrl = a.FileUrl,
            SecureUrl = a.CloudinarySecureUrl,
            MimeType = a.MimeType,
            FileSize = a.FileSize,
            Description = a.Description,
            CreatedAt = a.CreatedAt
        };
    }
}
