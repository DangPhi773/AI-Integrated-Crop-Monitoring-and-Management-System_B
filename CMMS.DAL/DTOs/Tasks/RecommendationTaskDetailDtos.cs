using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Tasks
{
    namespace CMMS.DAL.DTOs.Tasks
    {
        public class RecommendationTaskDetailRequest
        {
            public Guid? TaskId { get; set; }
            public Guid? SeasonId { get; set; } 
            public Guid? FarmId { get; set; }   
            public string? Title { get; set; }
            public decimal? Quantity { get; set; }
            public string? Status { get; set; }
            public string? Unit { get; set; }
            public string? Notes { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public List<Guid> AssignedToWorkerIds { get; set; } = new();
            public List<Guid> PlotIds { get; set; } = new();
            public List<Guid> BedIds { get; set; } = new();
        }

        public class RecommendationTaskDetailResponse
        {
            public Guid TaskDetailId { get; set; }
            public Guid? TaskId { get; set; }
            public string? TaskTitle { get; set; }
            public Guid? SeasonId { get; set; }
            public Guid? FarmId { get; set; }
            public string? Title { get; set; }
            public decimal? Quantity { get; set; }
            public string? Status { get; set; }
            public string? Unit { get; set; }
            public string? Notes { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public List<Guid> AssignedToWorkerIds { get; set; } = new();
            public List<Guid> PlotIds { get; set; } = new();
            public List<Guid> BedIds { get; set; } = new();

            public List<string>? WorkerNames { get; set; }
        }
    }
}
