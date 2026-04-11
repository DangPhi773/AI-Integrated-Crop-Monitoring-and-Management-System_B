using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class SubTaskMapper
    {
        public static SubTaskResponse ToResponse(SubTask s) => new()
        {
            SubTaskId = s.SubTaskId,
            Title = s.Title,
            Description = s.Description,
            TaskDetailId = s.TaskDetailId
        };
    }
}
