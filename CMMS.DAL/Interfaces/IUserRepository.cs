using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetByRoleNamesAsync(params string[] roleNames);
        System.Threading.Tasks.Task AddAsync(User user);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<bool> SaveChangesAsync();
    }
}
