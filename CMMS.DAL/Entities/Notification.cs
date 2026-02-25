using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Notification
{
    public Guid NoteId { get; set; }

    public Guid? PestDetectionId { get; set; }

    public Guid? UserId { get; set; }

    public string? NoteType { get; set; }

    public string? NoteTitle { get; set; }

    public string? NoteMessage { get; set; }

    public string? NoteStatus { get; set; }

    public DateTime? NoteCreatedAt { get; set; }

    public virtual PestDetection? PestDetection { get; set; }

    public virtual User? User { get; set; }
}
