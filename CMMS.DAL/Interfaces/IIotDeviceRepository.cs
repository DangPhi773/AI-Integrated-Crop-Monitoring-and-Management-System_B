using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;

namespace CMMS.DAL.Interfaces
{
    public interface IIotDeviceRepository
    {
        System.Threading.Tasks.Task<IEnumerable<IotDevice>> GetAllAsync();
        System.Threading.Tasks.Task<IotDevice?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(IotDevice entity);
        void Update(IotDevice entity);
        void Delete(IotDevice entity);
        System.Threading.Tasks.Task<bool> SaveChangesAsync();
    }
}
