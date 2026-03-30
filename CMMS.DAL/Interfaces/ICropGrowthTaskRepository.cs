using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ICropGrowthTaskRepository
    {
        Task<IEnumerable<CropGrowthTask>> GetAllAsync();
        Task<CropGrowthTask?> GetByIdAsync(Guid id);
        Task<IEnumerable<CropGrowthTask>> GetByStageIdAsync(Guid stageId);
        System.Threading.Tasks.Task AddAsync(CropGrowthTask entity);
        void Update(CropGrowthTask entity);
        void Delete(CropGrowthTask entity);
        Task<bool> SaveChangesAsync();
    }
}
