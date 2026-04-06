using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Entities
{
    public class SubTask
    {
        [Key]
        public Guid SubTaskId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid TaskDetailId { get; set; }

        [ForeignKey("TaskDetailId")]
        public virtual TaskDetail? TaskDetail { get; set; }
    }
}
