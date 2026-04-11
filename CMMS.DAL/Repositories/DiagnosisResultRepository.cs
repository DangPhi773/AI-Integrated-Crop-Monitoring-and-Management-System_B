using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class DiagnosisResultRepository : IDiagnosisResultRepository
    {
        private readonly AppDbContext _context;

        public DiagnosisResultRepository(AppDbContext context) => _context = context;

        public async Task<List<DiagnosisResult>> GetAllWithDetailsAsync()
        {
            return await _context.DiagnosisResults
                .AsNoTracking()
                .Include(d => d.Diagnoser)
                .Include(d => d.Report)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<DiagnosisResult?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.DiagnosisResults
                .AsNoTracking()
                .Include(d => d.Diagnoser)
                .Include(d => d.Report)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<DiagnosisResult>> GetByReportIdAsync(Guid reportId)
        {
            return await _context.DiagnosisResults
                .AsNoTracking()
                .Include(d => d.Diagnoser)
                .Where(d => d.ReportId == reportId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(DiagnosisResult entity) => await _context.DiagnosisResults.AddAsync(entity);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
