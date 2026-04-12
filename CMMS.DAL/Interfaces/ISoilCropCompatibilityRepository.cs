using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ISoilCropCompatibilityRepository
    {
        Task<IEnumerable<SoilCropCompatibility>> GetAllAsync();
        Task<SoilCropCompatibility?> GetByIdAsync(Guid id);
        Task<SoilCropCompatibility?> GetBySoilAndCropAsync(Guid soilId, Guid cropId);
        System.Threading.Tasks.Task AddAsync(SoilCropCompatibility entity);
        void Update(SoilCropCompatibility entity);
        void Delete(SoilCropCompatibility entity);
        Task<bool> SaveChangesAsync();
    }
}
