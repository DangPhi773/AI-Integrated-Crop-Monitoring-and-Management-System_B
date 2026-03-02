using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMS.DAL.DBContext;
using TaskEntity = CMMS.DAL.Entities.Task;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TasksController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
        {
            var tasks = await _db.Tasks
                .Include(t => t.TaskDetails)
                .AsNoTracking()
                .Select(t => ToDto(t))
                .ToListAsync();

            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDto>> GetById(Guid id)
        {
            var task = await _db.Tasks
                .Include(t => t.TaskDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TaskId == id);

            if (task == null) return NotFound();

            return Ok(ToDto(task));
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = new TaskEntity
            {
                TaskId = Guid.NewGuid(),
                TaskTitle = input.TaskTitle,
                TaskNotes = input.TaskNotes,
                TaskStatus = input.TaskStatus,
                TaskScheduledAt = input.TaskScheduledAt,
                AssignedToWorkerId = input.AssignedToWorkerId,
                SeasonId = input.SeasonId,
                TaskCreatedAt = DateTime.UtcNow
            };

            _db.Tasks.Add(entity);
            await _db.SaveChangesAsync();

            var dto = ToDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.TaskId }, dto);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TaskDto>> Update(Guid id, [FromBody] UpdateTaskDto input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.TaskId == id);
            if (entity == null) return NotFound();

            // Update allowed fields
            entity.TaskTitle = input.TaskTitle ?? entity.TaskTitle;
            entity.TaskNotes = input.TaskNotes ?? entity.TaskNotes;
            entity.TaskStatus = input.TaskStatus ?? entity.TaskStatus;
            entity.TaskScheduledAt = input.TaskScheduledAt ?? entity.TaskScheduledAt;
            entity.AssignedToWorkerId = input.AssignedToWorkerId ?? entity.AssignedToWorkerId;
            entity.SeasonId = input.SeasonId ?? entity.SeasonId;

            _db.Tasks.Update(entity);
            await _db.SaveChangesAsync();

            return Ok(ToDto(entity));
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _db.Tasks
                .Include(t => t.TaskDetails)
                .FirstOrDefaultAsync(t => t.TaskId == id);

            if (entity == null) return NotFound();

            // If you want cascade deletes for TaskDetails ensure DB is configured accordingly.
            // Here we explicitly remove details first to be safe.
            if (entity.TaskDetails?.Any() == true)
            {
                _db.TaskDetails.RemoveRange(entity.TaskDetails);
            }

            _db.Tasks.Remove(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // --- Mappers & DTOs ---

        private static TaskDto ToDto(TaskEntity t) =>
            new TaskDto
            {
                TaskId = t.TaskId,
                TaskTitle = t.TaskTitle,
                TaskNotes = t.TaskNotes,
                TaskStatus = t.TaskStatus,
                TaskScheduledAt = t.TaskScheduledAt,
                AssignedToWorkerId = t.AssignedToWorkerId,
                SeasonId = t.SeasonId,
                TaskCreatedAt = t.TaskCreatedAt,
                TaskDetails = t.TaskDetails?.Select(d => new TaskDetailDto
                {
                    TaskDetailId = d.TaskDetailId,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    Notes = d.Notes,
                    SeasonId = d.SeasonId
                }).ToList() ?? new List<TaskDetailDto>()
            };

        public record TaskDto
        {
            public Guid TaskId { get; init; }
            public string? TaskTitle { get; init; }
            public DateTime? TaskScheduledAt { get; init; }
            public string? TaskStatus { get; init; }
            public string? TaskNotes { get; init; }
            public DateTime? TaskCreatedAt { get; init; }
            public Guid? AssignedToWorkerId { get; init; }
            public Guid? SeasonId { get; init; }
            public List<TaskDetailDto> TaskDetails { get; init; } = new();
        }

        public record TaskDetailDto
        {
            public Guid TaskDetailId { get; init; }
            public Guid? SeasonId { get; init; }
            public DateTime? StartDate { get; init; }
            public DateTime? EndDate { get; init; }
            public string? Notes { get; init; }
        }

        public class CreateTaskDto
        {
            [Required]
            public string? TaskTitle { get; set; }

            public DateTime? TaskScheduledAt { get; set; }

            public string? TaskStatus { get; set; }

            public string? TaskNotes { get; set; }

            public Guid? AssignedToWorkerId { get; set; }

            public Guid? SeasonId { get; set; }
        }

        public class UpdateTaskDto
        {
            public string? TaskTitle { get; set; }
            public DateTime? TaskScheduledAt { get; set; }
            public string? TaskStatus { get; set; }
            public string? TaskNotes { get; set; }
            public Guid? AssignedToWorkerId { get; set; }
            public Guid? SeasonId { get; set; }
        }
    }
}
