using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMS.DAL.Repositories
{
    public class CropBedConfigRepository : ICropBedConfigRepository
    {
        private readonly AppDbContext _context;
        public CropBedConfigRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<CropBedConfig>> GetAllAsync()
            => await _context.CropBedConfigs.Include(c => c.Crop).AsNoTracking().ToListAsync();

        public async Task<CropBedConfig?> GetByIdAsync(Guid id)
            => await _context.CropBedConfigs.Include(c => c.Crop).FirstOrDefaultAsync(c => c.ConfigId == id);

        public async Task<IEnumerable<CropBedConfig>> GetByCropIdAsync(Guid cropId)
            => await _context.CropBedConfigs.AsNoTracking().Where(c => c.CropId == cropId).ToListAsync();

        public async Task<CropBedConfig?> GetByCropAndPatternAsync(Guid cropId, string pattern)
            => await _context.CropBedConfigs.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CropId == cropId && c.PlantingPattern == pattern);

        public async Task<CropBedConfig?> GetDefaultByCropAsync(Guid cropId)
            => await _context.CropBedConfigs.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CropId == cropId && c.IsDefault);

        public async System.Threading.Tasks.Task AddAsync(CropBedConfig entity)
            => await _context.CropBedConfigs.AddAsync(entity);

        public void Update(CropBedConfig entity) => _context.CropBedConfigs.Update(entity);
        public void Delete(CropBedConfig entity) => _context.CropBedConfigs.Remove(entity);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
