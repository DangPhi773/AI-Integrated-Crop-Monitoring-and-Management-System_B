using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ICropBedConfigRepository
    {
        Task<IEnumerable<CropBedConfig>> GetAllAsync();
        Task<CropBedConfig?> GetByIdAsync(Guid id);
        Task<IEnumerable<CropBedConfig>> GetByCropIdAsync(Guid cropId);
        Task<CropBedConfig?> GetByCropAndPatternAsync(Guid cropId, string pattern);
        Task<CropBedConfig?> GetDefaultByCropAsync(Guid cropId);
        System.Threading.Tasks.Task AddAsync(CropBedConfig entity);
        void Update(CropBedConfig entity);
        void Delete(CropBedConfig entity);
        Task<bool> SaveChangesAsync();
    }
}
