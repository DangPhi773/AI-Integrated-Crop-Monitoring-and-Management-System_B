using CMMS.DAL.DBContext;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMS.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Entities.Task>> GetAllAsync()
            => await _context.Tasks
                .Include(t => t.TaskDetails)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Entities.Task?> GetByIdAsync(Guid id)
            => await _context.Tasks
                .Include(t => t.TaskDetails)
                .FirstOrDefaultAsync(t => t.TaskId == id);

        public async System.Threading.Tasks.Task AddAsync(Entities.Task task)
            => await _context.Tasks.AddAsync(task);

        public void Update(Entities.Task task)
            => _context.Tasks.Update(task);

        public void Delete(Entities.Task task)
            => _context.Tasks.Remove(task);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
