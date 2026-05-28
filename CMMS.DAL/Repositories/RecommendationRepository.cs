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
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly AppDbContext _context;
        public RecommendationRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Recommendation>> GetAllWithDetailsAsync()
        {
            return await _context.Recommendations
                .Include(r => r.Diagnosis) 
                .Include(r => r.Season)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Recommendation?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Recommendations
                .Include(r => r.Diagnosis)
                .Include(r => r.Season)
                .FirstOrDefaultAsync(r => r.RecommendationId == id);
        }

        public async Task<IEnumerable<Recommendation>> GetByDiagnosisIdAsync(Guid diagnosisId)
        {
            return await _context.Recommendations
                .Include(r => r.Diagnosis)
                .Include(r => r.Season)
                .Where(r => r.DiagnosisId == diagnosisId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(Recommendation rec) => await _context.Recommendations.AddAsync(rec);
        public void Update(Recommendation rec) => _context.Recommendations.Update(rec);
        public void Delete(Recommendation rec) => _context.Recommendations.Remove(rec);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
