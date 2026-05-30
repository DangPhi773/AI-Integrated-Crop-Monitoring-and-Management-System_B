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
    public class SoilRepository : ISoilRepository
    {
        private readonly AppDbContext _context;
        public SoilRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Soil>> GetAllAsync()
        => await _context.Soils
            .Include(s => s.SoilCropCompatibilities)
                .ThenInclude(sc => sc.Crop)
            .Include(s => s.Plots)
            .OrderByDescending(s => s.CreatedAt)
            .ThenBy(s => s.Name)
            .AsNoTracking()
            .ToListAsync();

        public async Task<Soil?> GetByIdAsync(Guid id)
        => await _context.Soils
            .Include(s => s.SoilCropCompatibilities)
                .ThenInclude(sc => sc.Crop)
            .Include(s => s.Plots)
            .FirstOrDefaultAsync(s => s.SoilId == id);

        public async System.Threading.Tasks.Task AddAsync(Soil soil) => await _context.Soils.AddAsync(soil);

        public void Update(Soil soil) => _context.Soils.Update(soil);

        public void Delete(Soil soil) => _context.Soils.Remove(soil);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
