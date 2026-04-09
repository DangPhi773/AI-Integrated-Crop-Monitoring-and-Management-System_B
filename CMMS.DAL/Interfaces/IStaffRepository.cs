using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IStaffRepository
    {
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<User>> GetAllStaffsAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        System.Threading.Tasks.Task AddUserAsync(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task<bool> SaveChangesAsync();
    }
}
