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
    public class CropGrowthStageRepository : ICropGrowthStageRepository
    {
        private readonly AppDbContext _context;
        public CropGrowthStageRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<CropGrowthStage>> GetAllAsync() =>
            await _context.CropGrowthStages.Include(s => s.Crop).ToListAsync();

        public async Task<CropGrowthStage?> GetByIdAsync(Guid id) =>
            await _context.CropGrowthStages.Include(s => s.Crop).FirstOrDefaultAsync(s => s.StageId == id);

        public async Task<IEnumerable<CropGrowthStage>> GetByCropIdAsync(Guid cropId) =>
    await _context.CropGrowthStages
        .Include(s => s.Crop)
        .Where(s => s.CropId == cropId)
        .OrderBy(s => s.CreatedAt) 
        .ToListAsync();

        public async System.Threading.Tasks.Task AddAsync(CropGrowthStage stage) => await _context.CropGrowthStages.AddAsync(stage);
        public void Update(CropGrowthStage stage) => _context.CropGrowthStages.Update(stage);
        public void Delete(CropGrowthStage stage) => _context.CropGrowthStages.Remove(stage);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
