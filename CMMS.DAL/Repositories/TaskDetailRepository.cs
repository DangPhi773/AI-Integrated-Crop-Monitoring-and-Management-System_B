using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMMS.DAL.Repositories
{
    public class TaskDetailRepository : ITaskDetailRepository
    {
        private readonly AppDbContext _context;
        public TaskDetailRepository(AppDbContext context) => _context = context;

        private IQueryable<TaskDetail> BaseQuery()
            => _context.TaskDetails
                .Include(d => d.Task)
                .Include(d => d.AssignedToWorker)
                .Include(d => d.Bed)
                .Include(d => d.Season);

        public async System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetAllAsync()
            => await BaseQuery().AsNoTracking().ToListAsync();

        public async System.Threading.Tasks.Task<TaskDetail?> GetByIdAsync(Guid id)
            => await BaseQuery().FirstOrDefaultAsync(d => d.TaskDetailId == id);

        public async System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetBySeasonIdAsync(Guid seasonId)
            => await BaseQuery().Where(d => d.SeasonId == seasonId).AsNoTracking().ToListAsync();

        public async System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetByWorkerIdAsync(Guid workerId)
            => await BaseQuery().Where(d => d.AssignedToWorkerId == workerId).AsNoTracking().ToListAsync();

        public async System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetByBedIdAsync(Guid bedId)
            => await BaseQuery().Where(d => d.BedId == bedId).AsNoTracking().ToListAsync();

        public async System.Threading.Tasks.Task<IEnumerable<TaskDetail>> GetByTaskIdAsync(Guid taskId)
            => await BaseQuery().Where(d => d.TaskId == taskId).AsNoTracking().ToListAsync();

        public async System.Threading.Tasks.Task AddAsync(TaskDetail entity)
            => await _context.TaskDetails.AddAsync(entity);

        public void Update(TaskDetail entity)
            => _context.TaskDetails.Update(entity);

        public void Delete(TaskDetail entity)
            => _context.TaskDetails.Remove(entity);

        public async System.Threading.Tasks.Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
