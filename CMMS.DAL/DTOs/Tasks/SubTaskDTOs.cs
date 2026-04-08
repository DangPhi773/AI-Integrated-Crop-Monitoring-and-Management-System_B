using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Tasks
{
    public class SubTaskCreateRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid TaskDetailId { get; set; }
    }

    public class SubTaskUpdateRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class SubTaskResponse
    {
        public Guid SubTaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid TaskDetailId { get; set; }
    }
}
