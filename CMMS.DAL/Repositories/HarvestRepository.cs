using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class HarvestRepository : IHarvestRepository
    {
        private readonly AppDbContext _context;
        public HarvestRepository(AppDbContext context) => _context = context;

        private IQueryable<Harvest> WithIncludes()
            => _context.Harvests
                .Include(h => h.Plot)
                .Include(h => h.Season)
                .Include(h => h.Crop)
                .Include(h => h.HarvestDetails).ThenInclude(d => d.Bed)
                .Include(h => h.HarvestRecords);

        public async Task<IEnumerable<Harvest>> GetAllAsync()
            => await WithIncludes()
                .AsNoTracking()
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

        public async Task<Harvest?> GetByIdAsync(Guid id)
            => await WithIncludes()
                .FirstOrDefaultAsync(h => h.HarvestId == id);

        public async Task<IEnumerable<Harvest>> GetByPlotIdAsync(Guid plotId)
            => await WithIncludes()
                .Where(h => h.PlotId == plotId)
                .AsNoTracking()
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Harvest>> GetBySeasonIdAsync(Guid seasonId)
            => await WithIncludes()
                .Where(h => h.SeasonId == seasonId)
                .AsNoTracking()
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Harvest>> GetByFarmIdAsync(Guid farmId)
            => await WithIncludes()
                .Where(h => h.Plot.FarmId == farmId)
                .AsNoTracking()
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

        public async System.Threading.Tasks.Task AddAsync(Harvest entity)
            => await _context.Harvests.AddAsync(entity);

        public void Update(Harvest entity) => _context.Harvests.Update(entity);

        public void Delete(Harvest entity) => _context.Harvests.Remove(entity);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
