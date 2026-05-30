using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ICropRepository
    {
        System.Threading.Tasks.Task<IEnumerable<Crop>> GetAllAsync();
        System.Threading.Tasks.Task<Crop?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
        System.Threading.Tasks.Task AddAsync(Crop crop);
        void Update(Crop crop);
        void Delete(Crop crop);
        System.Threading.Tasks.Task<bool> SaveChangesAsync();
    }
}
