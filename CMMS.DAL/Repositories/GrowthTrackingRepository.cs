using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class GrowthTrackingRepository : IGrowthTrackingRepository
    {
        private readonly AppDbContext _context;
        public GrowthTrackingRepository(AppDbContext context) => _context = context;

        private IQueryable<GrowthTracking> WithIncludes()
            => _context.GrowthTrackings
                .Include(g => g.CropGrowthStage)
                .Include(g => g.HarvestDetail).ThenInclude(hd => hd.Bed)
                .Include(g => g.HarvestDetail).ThenInclude(hd => hd.Harvest).ThenInclude(h => h.Crop);

        public async Task<IEnumerable<GrowthTracking>> GetAllAsync()
            => await WithIncludes()
                .AsNoTracking()
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();

        public async Task<GrowthTracking?> GetByIdAsync(Guid id)
            => await WithIncludes()
                .FirstOrDefaultAsync(g => g.TrackingId == id);

        public async Task<IEnumerable<GrowthTracking>> GetByHarvestDetailIdAsync(Guid harvestDetailId)
            => await WithIncludes()
                .Where(g => g.HarvestDetailId == harvestDetailId)
                .AsNoTracking()
                .OrderBy(g => g.StartDate)
                .ToListAsync();

        public async Task<IEnumerable<GrowthTracking>> GetBySeasonIdAsync(Guid seasonId)
            => await WithIncludes()
                .Where(g => g.HarvestDetail.Harvest.SeasonId == seasonId)
                .AsNoTracking()
                .OrderBy(g => g.StartDate)
                .ToListAsync();

        public async Task<GrowthTracking?> GetCurrentByHarvestDetailIdAsync(Guid harvestDetailId)
            => await _context.GrowthTrackings
                .Include(g => g.CropGrowthStage)
                .Where(g => g.HarvestDetailId == harvestDetailId && g.TrackingStatus == "In-Progress")
                .OrderByDescending(g => g.StartDate)
                .FirstOrDefaultAsync();

        public async System.Threading.Tasks.Task AddAsync(GrowthTracking tracking)
            => await _context.GrowthTrackings.AddAsync(tracking);

        public void Update(GrowthTracking tracking) => _context.GrowthTrackings.Update(tracking);

        public void Delete(GrowthTracking tracking) => _context.GrowthTrackings.Remove(tracking);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
