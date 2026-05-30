using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IFarmRepository
    {
        Task<IEnumerable<Farm>> GetAllAsync();
        Task<Farm?> GetByIdAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
        System.Threading.Tasks.Task AddAsync(Farm farm);
        void Update(Farm farm);
        void Delete(Farm farm);
        Task<bool> SaveChangesAsync();
    }
}
