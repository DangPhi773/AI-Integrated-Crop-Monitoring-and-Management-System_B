using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ICropGrowthStageRepository
    {
        Task<IEnumerable<CropGrowthStage>> GetAllAsync();
        Task<CropGrowthStage?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(CropGrowthStage stage);
        void Update(CropGrowthStage stage);
        void Delete(CropGrowthStage stage);
        Task<bool> SaveChangesAsync();
    }
}
