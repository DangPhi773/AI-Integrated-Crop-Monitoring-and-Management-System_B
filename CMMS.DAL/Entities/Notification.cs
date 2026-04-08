using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Notification")]
public partial class Notification
{
    [Key]
    public Guid NoteId { get; set; }

    public Guid? PestDetectionId { get; set; }

    public Guid? UserId { get; set; }

    [MaxLength(100)]
    public string? NoteType { get; set; }

    [MaxLength(255)]
    public string? NoteTitle { get; set; }

    public string? NoteMessage { get; set; }

    [MaxLength(50)]
    public string? NoteStatus { get; set; } 

    [Column(TypeName = "timestamp")]
    public DateTime? NoteCreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("PestDetectionId")]
    public virtual PestDetection? PestDetection { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}