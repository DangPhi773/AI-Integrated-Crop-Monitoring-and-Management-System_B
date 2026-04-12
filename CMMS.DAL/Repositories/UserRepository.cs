using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMMS.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetByRoleNamesAsync(params string[] roleNames)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Role)
                .Where(u => u.Role != null && roleNames.Contains(u.Role.RoleName) && u.Email != null)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName.ToLower().Trim() == roleName.ToLower().Trim());
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.OrderBy(r => r.RoleName).ToListAsync();
        }
    }
}