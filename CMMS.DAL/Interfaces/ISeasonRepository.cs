using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ISeasonRepository
    {
        System.Threading.Tasks.Task<IEnumerable<Season>> GetAllAsync();
        System.Threading.Tasks.Task<Season?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
        System.Threading.Tasks.Task AddAsync(Season season);
        void Update(Season season);
        void Delete(Season season);
        System.Threading.Tasks.Task<bool> SaveChangesAsync();
    }
}
