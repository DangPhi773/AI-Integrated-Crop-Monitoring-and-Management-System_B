using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Tasks
{
    public class TaskRequest
    {
        public string? TaskTitle { get; set; }
        public DateTime? TaskScheduledAt { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskNotes { get; set; }
        public List<TaskDetailRequest>? TaskDetails { get; set; }
    }

    public class TaskResponse
    {
        public Guid TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public DateTime? TaskScheduledAt { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskNotes { get; set; }
        public DateTime? TaskCreatedAt { get; set; }
        public List<TaskDetailDto> TaskDetails { get; set; } = new List<TaskDetailDto>();
    }

    public class TaskDetailRequest
    {
        public Guid? TaskDetailId { get; set; } 
        public Guid? SeasonId { get; set; }
        public Guid? AssignedToWorkerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
    }

    public class TaskDetailDto : TaskDetailRequest
    {
        public Guid TaskDetailId { get; set; }
        public string? WorkerName { get; set; }
    }
}
