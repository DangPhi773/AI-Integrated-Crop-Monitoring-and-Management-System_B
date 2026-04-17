using CMMS.DAL.DTOs.Tasks;
using Task = CMMS.DAL.Entities.Task;

namespace CMMS.BLL.Mappings
{
    public static class TaskMapper
    {
        public static TaskResponse ToResponse(Task t) => new()
        {
            TaskId = t.TaskId,
            TaskTitle = t.TaskTitle,
            TaskStatus = t.TaskStatus,
            TaskNotes = t.TaskNotes,
            TaskType = t.TaskType,
            TaskCreatedAt = t.TaskCreatedAt,
            TaskScheduledAt = t.TaskScheduledAt,
            TaskDetailsCount = t.TaskDetails?.Count ?? 0
        };

        public static Task ToEntity(TaskRequest request) => new()
        {
            TaskId = Guid.NewGuid(), 
            TaskTitle = request.TaskTitle,
            TaskStatus = request.TaskStatus,
            TaskNotes = request.TaskNotes,
            TaskType = request.TaskType,
            TaskScheduledAt = request.TaskScheduledAt ?? DateTime.UtcNow,
            TaskCreatedAt = DateTime.UtcNow
        };
    }
}
