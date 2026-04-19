using System;
using System.Collections.Generic;

namespace CMMS.DAL.DTOs.Tasks
{
    // ===== TASK (mẫu công việc, dùng lại được) =====

    public class TaskRequest
    {
        public string? TaskTitle { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskNotes { get; set; }
        public string? TaskType { get; set; }
    }

    public class TaskResponse
    {
        public Guid TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskNotes { get; set; }
        public string? TaskType { get; set; }
        public DateTime? TaskCreatedAt { get; set; }
        public DateTime? TaskScheduledAt { get; set; }
        public int TaskDetailsCount { get; set; }
    }

    // ===== TASK DETAIL (chi tiết: ai làm, ở đâu, khi nào) =====

    public class TaskDetailRequest
    {
        public Guid? TaskId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? FarmId { get; set; }
        public List<Guid>? AssignedToWorkerIds { get; set; }
        public List<Guid>? BedIds { get; set; }
        public List<Guid>? PlotIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
    }

    public class TaskDetailResponse
    {
        public Guid TaskDetailId { get; set; }
        public Guid? TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? FarmId { get; set; }
        public List<Guid> AssignedToWorkerIds { get; set; } = new();
        public List<Guid> BedIds { get; set; } = new();
        public List<Guid> PlotIds { get; set; } = new();
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
    }

    public class UpdateTaskDetailStatusRequest
    {
        public string Status { get; set; } = null!;
    }
}
