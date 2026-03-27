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
    public class CropGrowthTaskRepository : ICropGrowthTaskRepository
    {
        private readonly AppDbContext _context;

        public CropGrowthTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        private IQueryable<CropGrowthTask> BaseQuery() =>
            _context.CropGrowthTasks
                    .Include(t => t.CropGrowthStage);

        public async Task<IEnumerable<CropGrowthTask>> GetAllAsync()
        {
            return await BaseQuery()
                .AsNoTracking()
                .OrderBy(t => t.Priority)
                .ToListAsync();
        }

        public async Task<CropGrowthTask?> GetByIdAsync(Guid id)
        {
            return await BaseQuery()
                .FirstOrDefaultAsync(t => t.GrowthTaskId == id);
        }

        public async Task<IEnumerable<CropGrowthTask>> GetByStageIdAsync(Guid stageId)
        {
            return await BaseQuery()
                .Where(t => t.StageId == stageId)
                .OrderBy(t => t.Priority)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(CropGrowthTask entity)
        {
            await _context.CropGrowthTasks.AddAsync(entity);
        }

        public void Update(CropGrowthTask entity)
        {
            _context.CropGrowthTasks.Update(entity);
        }

        public void Delete(CropGrowthTask entity)
        {
            _context.CropGrowthTasks.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
