using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.DAL.Repositories
{
    public class ReportEnvironmentSnapshotRepository : IReportEnvironmentSnapshotRepository
    {
        private readonly AppDbContext _context;

        public ReportEnvironmentSnapshotRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task AddAsync(ReportEnvironmentSnapshot entity) => await _context.ReportEnvironmentSnapshots.AddAsync(entity);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
