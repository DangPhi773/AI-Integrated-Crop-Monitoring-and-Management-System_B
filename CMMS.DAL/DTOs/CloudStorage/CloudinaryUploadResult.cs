namespace CMMS.DAL.DTOs.CloudStorage
{
    public class CloudinaryUploadResult
    {
        public string PublicId { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string SecureUrl { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public long FileSize { get; set; }
    }
}
