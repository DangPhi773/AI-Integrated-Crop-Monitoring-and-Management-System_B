using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.DAL.Repositories
{
    public class DiagnosisResultRepository : IDiagnosisResultRepository
    {
        private readonly AppDbContext _context;

        public DiagnosisResultRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task AddAsync(DiagnosisResult entity) => await _context.DiagnosisResults.AddAsync(entity);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
