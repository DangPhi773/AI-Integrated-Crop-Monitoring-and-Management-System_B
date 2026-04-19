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
    public class RecommendationTaskRepository : IRecommendationTaskRepository
    {
        private readonly AppDbContext _context;
        public RecommendationTaskRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<RecommendationTask>> GetAllAsync() =>
            await _context.RecommendationTasks
                .Include(t => t.CreatedByOwner)
                .Include(t => t.AssignedToWorker)
                .Include(t => t.RecommendationTaskDetails)
                .ToListAsync();

        public async Task<RecommendationTask?> GetByIdAsync(Guid id) =>
            await _context.RecommendationTasks
                .Include(t => t.CreatedByOwner)
                .Include(t => t.AssignedToWorker)
                .Include(t => t.RecommendationTaskDetails)
                .FirstOrDefaultAsync(t => t.RecommendationTaskId == id);

        public async System.Threading.Tasks.Task AddAsync(RecommendationTask task) => await _context.RecommendationTasks.AddAsync(task);
        public void Update(RecommendationTask task) => _context.RecommendationTasks.Update(task);
        public void Delete(RecommendationTask task) => _context.RecommendationTasks.Remove(task);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
