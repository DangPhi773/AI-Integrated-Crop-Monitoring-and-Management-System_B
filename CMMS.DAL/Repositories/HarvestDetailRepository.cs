using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class HarvestDetailRepository : IHarvestDetailRepository
    {
        private readonly AppDbContext _context;
        public HarvestDetailRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<HarvestDetail>> GetAllAsync()
            => await _context.HarvestDetails
                .Include(d => d.Bed)
                .Include(d => d.Harvest).ThenInclude(h => h.Crop)
                .Include(d => d.Harvest).ThenInclude(h => h.Season)
                .Include(d => d.Harvest).ThenInclude(h => h.Plot)
                .AsNoTracking()
                .ToListAsync();

        public async Task<HarvestDetail?> GetByIdAsync(Guid id)
            => await _context.HarvestDetails
                .Include(d => d.Bed)
                .Include(d => d.Harvest).ThenInclude(h => h.Crop)
                .Include(d => d.Harvest).ThenInclude(h => h.Season)
                .Include(d => d.Harvest).ThenInclude(h => h.Plot)
                .FirstOrDefaultAsync(d => d.HarvestDetailId == id);

        public async Task<IEnumerable<HarvestDetail>> GetByHarvestIdAsync(Guid harvestId)
            => await _context.HarvestDetails
                .Include(d => d.Bed)
                .Include(d => d.Harvest).ThenInclude(h => h.Crop)
                .Include(d => d.Harvest).ThenInclude(h => h.Season)
                .Include(d => d.Harvest).ThenInclude(h => h.Plot)
                .Where(d => d.HarvestId == harvestId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<HarvestDetail>> GetBySeasonIdAsync(Guid seasonId)
            => await _context.HarvestDetails
                .Include(d => d.Bed)
                .Include(d => d.Harvest).ThenInclude(h => h.Crop)
                .Where(d => d.Harvest.SeasonId == seasonId)
                .AsNoTracking()
                .ToListAsync();

        public async System.Threading.Tasks.Task AddAsync(HarvestDetail entity)
            => await _context.HarvestDetails.AddAsync(entity);

        public async System.Threading.Tasks.Task AddRangeAsync(IEnumerable<HarvestDetail> entities)
            => await _context.HarvestDetails.AddRangeAsync(entities);

        public void Update(HarvestDetail entity) => _context.HarvestDetails.Update(entity);

        public void Delete(HarvestDetail entity) => _context.HarvestDetails.Remove(entity);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
