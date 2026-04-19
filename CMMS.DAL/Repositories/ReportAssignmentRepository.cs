using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class ReportAssignmentRepository : IReportAssignmentRepository
    {
        private readonly AppDbContext _context;

        public ReportAssignmentRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task AddAsync(ReportAssignment entity) => await _context.ReportAssignments.AddAsync(entity);

        public async Task<ReportAssignment?> GetLatestByReportAndUserAsync(Guid reportId, Guid userId)
        {
            return await _context.ReportAssignments
                .Where(a => a.ReportId == reportId && a.AssignedTo == userId)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
