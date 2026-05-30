using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class BedRepository : IBedRepository
    {
        private readonly AppDbContext _context;
        public BedRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Bed>> GetAllAsync()
            => await _context.Beds
                .Include(b => b.Plot)
                .Include(b => b.HarvestDetails)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Bed?> GetByIdAsync(Guid id)
            => await _context.Beds
                .Include(b => b.Plot)
                .Include(b => b.HarvestDetails)
                .FirstOrDefaultAsync(b => b.BedId == id);

        public async System.Threading.Tasks.Task AddAsync(Bed bed) => await _context.Beds.AddAsync(bed);

        public async System.Threading.Tasks.Task AddRangeAsync(IEnumerable<Bed> beds) => await _context.Beds.AddRangeAsync(beds);

        public Task<bool> AnyByPlotIdAsync(Guid plotId) => _context.Beds.AnyAsync(b => b.PlotId == plotId);

        public void Update(Bed bed) => _context.Beds.Update(bed);

        public void Delete(Bed bed) => _context.Beds.Remove(bed);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;

        public async Task<IEnumerable<Bed>> GetBedsByPlotIdAsync(Guid plotId)
        {
            return await _context.Beds
                .Include(b => b.Plot)
                .Include(b => b.HarvestDetails)
                .Where(b => b.PlotId == plotId)
                .ToListAsync();
        }
    }
}
