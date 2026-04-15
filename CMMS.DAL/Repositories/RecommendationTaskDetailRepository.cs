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
    public class RecommendationTaskDetailRepository : IRecommendationTaskDetailRepository
    {
        private readonly AppDbContext _context;
        public RecommendationTaskDetailRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<RecommendationTaskDetail>> GetAllAsync() =>
            await _context.RecommendationTaskDetails
                .Include(d => d.Task)
                .Include(d => d.Season) 
                .Include(d => d.Farm) 
                .ToListAsync();

        public async Task<IEnumerable<RecommendationTaskDetail>> GetByTaskIdAsync(Guid taskId) =>
            await _context.RecommendationTaskDetails
                .Include(d => d.Task)
                .Where(d => d.TaskId == taskId)
                .ToListAsync();

        public async Task<RecommendationTaskDetail?> GetByIdAsync(Guid id) =>
            await _context.RecommendationTaskDetails
                .Include(d => d.Task)
                .Include(d => d.Season)
                .Include(d => d.Farm)
                .FirstOrDefaultAsync(d => d.TaskDetailId == id);

        public async System.Threading.Tasks.Task AddAsync(RecommendationTaskDetail detail) => await _context.RecommendationTaskDetails.AddAsync(detail);
        public void Update(RecommendationTaskDetail detail) => _context.RecommendationTaskDetails.Update(detail);
        public void Delete(RecommendationTaskDetail detail) => _context.RecommendationTaskDetails.Remove(detail);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
