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
    public class CropRepository : ICropRepository
    {
        private readonly AppDbContext _context;
        public CropRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task<IEnumerable<Crop>> GetAllAsync()
            => await _context.Crops
                .Include(c => c.SoilCropCompatibilities)
                    .ThenInclude(sc => sc.Soil)
                .OrderByDescending(c => c.CreatedAt)
                .ThenBy(c => c.CropName)
                .AsNoTracking()
                .ToListAsync();

        public async System.Threading.Tasks.Task<Crop?> GetByIdAsync(Guid id)
            => await _context.Crops
                .Include(c => c.SoilCropCompatibilities)
                    .ThenInclude(sc => sc.Soil)
                .FirstOrDefaultAsync(c => c.CropId == id);

        public async System.Threading.Tasks.Task AddAsync(Crop crop) => await _context.Crops.AddAsync(crop);

        public void Update(Crop crop) => _context.Crops.Update(crop);

        public void Delete(Crop crop) => _context.Crops.Remove(crop);

        public async System.Threading.Tasks.Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
