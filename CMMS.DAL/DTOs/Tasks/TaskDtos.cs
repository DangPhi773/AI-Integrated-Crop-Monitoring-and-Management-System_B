using System;
using System.Collections.Generic;

namespace CMMS.DAL.DTOs.Tasks
{
    // ===== Task (mẫu công việc) =====
    public class TaskRequest
    {
        public string? TaskTitle { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskNotes { get; set; }
    }

    public class TaskResponse
    {
        public Guid TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskNotes { get; set; }
        public DateTime? TaskCreatedAt { get; set; }
        public int TaskDetailsCount { get; set; }
    }

    // ===== TaskDetail (chi tiết công việc thực tế) =====
    public class TaskDetailRequest
    {
        public Guid? TaskId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? AssignedToWorkerId { get; set; }
        public Guid? BedId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
    }

    public class TaskDetailResponse
    {
        public Guid TaskDetailId { get; set; }
        public Guid? TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? AssignedToWorkerId { get; set; }
        public string? WorkerName { get; set; }
        public Guid? BedId { get; set; }
        public string? BedName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
    }
}
