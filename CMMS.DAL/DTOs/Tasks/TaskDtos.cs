using System;
using System.Collections.Generic;

namespace CMMS.DAL.DTOs.Tasks
{
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

    public class TaskDetailRequest
    {
        public Guid? TaskId { get; set; }
        public Guid? SeasonId { get; set; }

        // --- ĐÃ UPDATE SANG MẢNG ID ---
        public List<Guid>? AssignedToWorkerIds { get; set; } = new List<Guid>();
        public List<Guid>? BedIds { get; set; } = new List<Guid>();
        public List<Guid>? PlotIds { get; set; } = new List<Guid>();
        // ------------------------------

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

        // --- ĐÃ UPDATE SANG MẢNG ID ---
        public List<Guid> AssignedToWorkerIds { get; set; } = new List<Guid>();
        public List<Guid> BedIds { get; set; } = new List<Guid>();
        public List<Guid> PlotIds { get; set; } = new List<Guid>();
        // ------------------------------

        public string? WorkerName { get; set; } 
        public string? BedName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
    }
}
