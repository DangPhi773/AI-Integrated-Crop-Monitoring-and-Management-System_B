using CMMS.DAL.DBContext;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task AddAsync(Entities.Task task)
        => await _context.Tasks.AddAsync(task);

        public void Delete(Entities.Task task)
        => _context.Tasks.Remove(task);

        public async Task<IEnumerable<Entities.Task>> GetAllAsync()
        => await _context.Tasks
                .Include(t => t.TaskDetails)
                    .ThenInclude(d => d.AssignedToWorker) 
                .AsNoTracking()
                .ToListAsync();

        public async Task<Entities.Task?> GetByIdAsync(Guid id)
        => await _context.Tasks
                .Include(t => t.TaskDetails)
                    .ThenInclude(d => d.AssignedToWorker)
                .FirstOrDefaultAsync(t => t.TaskId == id);

        public async Task<bool> SaveChangesAsync()
        => await _context.SaveChangesAsync() > 0;

        public void Update(Entities.Task task)
        => _context.Tasks.Update(task);
    }
}
