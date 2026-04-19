using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Repositories
{
    public class SubTaskRepository : ISubTaskRepository
    {
        private readonly AppDbContext _context;
        public SubTaskRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<SubTask>> GetAllByTaskDetailIdAsync(Guid taskDetailId)
            => await _context.SubTasks.Where(s => s.TaskDetailId == taskDetailId).ToListAsync();

        public async Task<SubTask?> GetByIdAsync(Guid id) => await _context.SubTasks.FindAsync(id);

        public async System.Threading.Tasks.Task AddAsync(SubTask subTask) => await _context.SubTasks.AddAsync(subTask);

        public void Update(SubTask subTask) => _context.SubTasks.Update(subTask);

        public void Delete(SubTask subTask) => _context.SubTasks.Remove(subTask);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
