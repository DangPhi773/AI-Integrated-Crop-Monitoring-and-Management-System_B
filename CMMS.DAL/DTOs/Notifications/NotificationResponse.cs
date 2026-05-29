namespace CMMS.DAL.DTOs.Notifications
{
    public class NotificationResponse
    {
        public Guid NoteId { get; set; }
        public Guid? ReportId { get; set; }
        public Guid? DiagnosisId { get; set; }
        public string? NoteType { get; set; }
        public string? NoteTitle { get; set; }
        public string? NoteMessage { get; set; }
        public string? NoteStatus { get; set; }
        public DateTime? NoteCreatedAt { get; set; }
    }
}
