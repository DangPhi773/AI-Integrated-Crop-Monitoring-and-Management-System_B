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
        Task<bool> EmailExistsAsync(string email);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task<IEnumerable<User>> GetUsersWithoutRoleAsync();
        Task<bool> SaveChangesAsync();
        Task<User?> GetProfileByIdAsync(Guid id);
    }
}
