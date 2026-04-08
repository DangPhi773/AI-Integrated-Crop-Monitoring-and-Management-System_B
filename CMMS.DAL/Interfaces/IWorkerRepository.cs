using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IWorkerRepository
    {
        System.Threading.Tasks.Task<Role?> GetRoleByNameAsync(string roleName);
        System.Threading.Tasks.Task<IEnumerable<User>> GetAllWorkersAsync();
        System.Threading.Tasks.Task<User?> GetWorkerByIdAsync(Guid id);
        System.Threading.Tasks.Task AddWorkerAsync(User worker);
        System.Threading.Tasks.Task<bool> EmailExistsAsync(string email);
        void UpdateWorker(User worker);
        void DeleteWorker(User worker);
        System.Threading.Tasks.Task<bool> SaveChangesAsync();
    }
}
