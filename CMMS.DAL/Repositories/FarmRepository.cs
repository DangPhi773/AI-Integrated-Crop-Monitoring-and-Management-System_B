using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Repositories
{
    public class FarmRepository : IFarmRepository
    {
        private readonly AppDbContext _context;
        public FarmRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Farm>> GetAllAsync()
            => await _context.Farms
                .Include(f => f.Seasons)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Farm?> GetByIdAsync(Guid id)
            => await _context.Farms
                .Include(f => f.Seasons)
                .FirstOrDefaultAsync(f => f.FarmId == id);

        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var normalized = name.Trim().ToLower();
            return await _context.Farms
                .AsNoTracking()
                .AnyAsync(f => f.FarmName.Trim().ToLower() == normalized
                            && (excludeId == null || f.FarmId != excludeId));
        }

        public async System.Threading.Tasks.Task AddAsync(Farm farm) => await _context.Farms.AddAsync(farm);

        public void Update(Farm farm) => _context.Farms.Update(farm);

        public void Delete(Farm farm) => _context.Farms.Remove(farm);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;

    }
}
