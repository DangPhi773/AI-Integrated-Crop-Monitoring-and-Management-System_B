using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class Attachment
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ObjectType { get; set; } = null!;

    public Guid ObjectId { get; set; }

    [MaxLength(50)]
    public string? AttachmentType { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = null!;

    [Required]
    public string FileUrl { get; set; } = null!;

    [MaxLength(255)]
    public string? CloudinaryPublicId { get; set; }

    public string? CloudinarySecureUrl { get; set; }

    [MaxLength(20)]
    public string? FileExtension { get; set; }

    [MaxLength(100)]
    public string? MimeType { get; set; }

    public long? FileSize { get; set; }

    public Guid UploadedBy { get; set; }

    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UploadedBy")]
    public virtual User? Uploader { get; set; }
}
