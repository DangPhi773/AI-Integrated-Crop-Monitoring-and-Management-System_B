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
    public class SoilCropCompatibilityRepository : ISoilCropCompatibilityRepository
    {
        private readonly AppDbContext _context;
        public SoilCropCompatibilityRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<SoilCropCompatibility>> GetAllAsync() =>
            await _context.SoilCropCompatibilities
                .Include(s => s.Soil)
                .Include(s => s.Crop)
                .ToListAsync();

        public async Task<SoilCropCompatibility?> GetByIdAsync(Guid id) =>
            await _context.SoilCropCompatibilities
                .Include(s => s.Soil)
                .Include(s => s.Crop)
                .FirstOrDefaultAsync(x => x.ComptId == id);

        public async Task<SoilCropCompatibility?> GetBySoilAndCropAsync(Guid soilId, Guid cropId) =>
            await _context.SoilCropCompatibilities
                .FirstOrDefaultAsync(x => x.SoilId == soilId && x.CropId == cropId);

        public async System.Threading.Tasks.Task AddAsync(SoilCropCompatibility entity) => await _context.SoilCropCompatibilities.AddAsync(entity);
        public void Update(SoilCropCompatibility entity) => _context.SoilCropCompatibilities.Update(entity);
        public void Delete(SoilCropCompatibility entity) => _context.SoilCropCompatibilities.Remove(entity);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
