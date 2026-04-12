using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class TaskDetailMapper
    {
        public static TaskDetailResponse ToResponse(TaskDetail d) => new()
        {
            TaskDetailId = d.TaskDetailId,
            TaskId = d.TaskId,
            TaskTitle = d.Task?.TaskTitle,
            SeasonId = d.SeasonId,
            AssignedToWorkerIds = d.AssignedToWorkerIds,
            BedIds = d.BedIds,
            PlotIds = d.PlotIds,
            StartDate = d.StartDate,
            EndDate = d.EndDate,
            Notes = d.Notes,
            Status = d.Status
        };
    }
}
