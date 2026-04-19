namespace CMMS.DAL.DTOs.WorkerSchedules
{
    public class WorkerScheduleResponse
    {
        public Guid ScheduleId { get; set; }
        public Guid? TaskDetailId { get; set; }
        public Guid? WorkerId { get; set; }
        public string? WorkerName { get; set; }
        public string? TaskTitle { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TaskDetailStatus { get; set; }
    }
}
