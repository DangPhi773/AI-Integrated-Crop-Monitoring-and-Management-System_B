namespace CMMS.DAL.DTOs.Attachments
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public string ObjectType { get; set; } = null!;
        public Guid ObjectId { get; set; }
        public string? AttachmentType { get; set; }
        public string FileName { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string? SecureUrl { get; set; }
        public string? MimeType { get; set; }
        public long? FileSize { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
