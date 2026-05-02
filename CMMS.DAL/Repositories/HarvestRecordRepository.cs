using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class HarvestRecordRepository : IHarvestRecordRepository
    {
        private readonly AppDbContext _context;
        public HarvestRecordRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<HarvestRecord>> GetAllAsync()
            => await _context.HarvestRecords
                .AsNoTracking()
                .OrderByDescending(r => r.HarvestDate)
                .ToListAsync();

        public async Task<HarvestRecord?> GetByIdAsync(Guid id)
            => await _context.HarvestRecords
                .FirstOrDefaultAsync(r => r.HarvestRecordId == id);

        public async Task<IEnumerable<HarvestRecord>> GetByHarvestIdAsync(Guid harvestId)
            => await _context.HarvestRecords
                .Where(r => r.HarvestId == harvestId)
                .AsNoTracking()
                .OrderByDescending(r => r.HarvestDate)
                .ToListAsync();

        public async System.Threading.Tasks.Task AddAsync(HarvestRecord entity)
            => await _context.HarvestRecords.AddAsync(entity);

        public void Update(HarvestRecord entity) => _context.HarvestRecords.Update(entity);

        public void Delete(HarvestRecord entity) => _context.HarvestRecords.Remove(entity);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
