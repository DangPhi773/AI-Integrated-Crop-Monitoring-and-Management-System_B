using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        System.Threading.Tasks.Task AddAsync(User user);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<bool> SaveChangesAsync();
    }
}
