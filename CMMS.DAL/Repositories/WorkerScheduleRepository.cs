using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class WorkerScheduleRepository : IWorkerScheduleRepository
    {
        private readonly AppDbContext _context;
        public WorkerScheduleRepository(AppDbContext context) => _context = context;

        private IQueryable<WorkerSchedule> BaseQuery()
            => _context.WorkerSchedules
                .Include(ws => ws.TaskDetail)
                    .ThenInclude(td => td!.Task)
                .Include(ws => ws.Worker);

        public async Task<IEnumerable<WorkerSchedule>> GetByWorkerIdAsync(Guid workerId)
            => await BaseQuery()
                .Where(ws => ws.WorkerId == workerId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<WorkerSchedule>> GetByTaskDetailIdAsync(Guid taskDetailId)
            => await BaseQuery()
                .Where(ws => ws.TaskDetailId == taskDetailId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<WorkerSchedule?> GetByIdAsync(Guid id)
            => await BaseQuery().FirstOrDefaultAsync(ws => ws.ScheduleId == id);

        public async Task<WorkerSchedule?> GetByTaskDetailAndWorkerAsync(Guid taskDetailId, Guid workerId)
            => await _context.WorkerSchedules
                .FirstOrDefaultAsync(ws => ws.TaskDetailId == taskDetailId && ws.WorkerId == workerId);

        public async System.Threading.Tasks.Task AddAsync(WorkerSchedule entity)
            => await _context.WorkerSchedules.AddAsync(entity);

        public async System.Threading.Tasks.Task AddRangeAsync(IEnumerable<WorkerSchedule> entities)
            => await _context.WorkerSchedules.AddRangeAsync(entities);

        public void Delete(WorkerSchedule entity)
            => _context.WorkerSchedules.Remove(entity);

        public void DeleteRange(IEnumerable<WorkerSchedule> entities)
            => _context.WorkerSchedules.RemoveRange(entities);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
