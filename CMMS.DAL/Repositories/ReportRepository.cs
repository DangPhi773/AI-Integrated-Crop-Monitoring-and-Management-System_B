using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(Guid id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task<IEnumerable<Report>> GetAllWithDetailsAsync()
        {
            return await _context.Reports
                .AsNoTracking()
                .Include(r => r.Creator)
                .Include(r => r.Owner)
                .Include(r => r.EnvironmentSnapshots)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Report?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Reports
                .AsNoTracking()
                .Include(r => r.Creator)
                .Include(r => r.Owner)
                .Include(r => r.EnvironmentSnapshots)
                .FirstOrDefaultAsync(r => r.ReportId == id);
        }

        public async System.Threading.Tasks.Task AddAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
        }

        public void Update(Report report)
        {
            _context.Reports.Update(report);
        }

        public void Delete(Report report)
        {
            _context.Reports.Remove(report);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
