using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetAllAsync();
        System.Threading.Tasks.Task<Entities.Task?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(Entities.Task task);
        void Update(Entities.Task task);
        void Delete(Entities.Task task);
        System.Threading.Tasks.Task<bool> SaveChangesAsync();
    }
}
