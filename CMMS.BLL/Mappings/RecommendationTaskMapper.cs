using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Mappings
{
    public static class RecommendationTaskMapper
    {
        public static RecommendationTaskResponse ToResponse(RecommendationTask t) => new()
        {
            RecommendationTaskId = t.RecommendationTaskId,
            Title = t.Title,
            TaskStatus = t.TaskStatus,
            TaskScheduledAt = t.TaskScheduledAt,
            TaskCreatedAt = t.TaskCreatedAt,
            OwnerName = t.CreatedByOwner?.Fullname, 
            WorkerName = t.AssignedToWorker?.Fullname,
            TotalDetails = t.RecommendationTaskDetails?.Count ?? 0
        };

        public static IEnumerable<RecommendationTaskResponse> ToResponseList(IEnumerable<RecommendationTask> list)
            => list.Select(ToResponse);
    }
}
