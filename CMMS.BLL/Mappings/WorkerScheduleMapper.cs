using CMMS.DAL.DTOs.WorkerSchedules;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class WorkerScheduleMapper
    {
        public static WorkerScheduleResponse ToResponse(WorkerSchedule ws) => new()
        {
            ScheduleId = ws.ScheduleId,
            TaskDetailId = ws.TaskDetailId,
            WorkerId = ws.WorkerId,
            WorkerName = ws.Worker?.Fullname,
            TaskTitle = ws.TaskDetail?.Task?.TaskTitle,
            Description = ws.Description,
            Status = ws.TaskDetail?.Status,
            StartDate = ws.TaskDetail?.StartDate,
            EndDate = ws.TaskDetail?.EndDate
        };
    }
}
