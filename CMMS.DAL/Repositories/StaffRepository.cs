using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _context;
        public StaffRepository(AppDbContext context) => _context = context;

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName.ToLower().Trim() == roleName.ToLower().Trim());
        }

        public async Task<IEnumerable<User>> GetAllStaffsAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Status.ToUpper() == "ACTIVE" &&
               u.Role != null &&
               (u.Role.RoleName.ToUpper() == "WORKER" || u.Role.RoleName.ToUpper() == "SPECIALIST"))
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id && u.Status.ToUpper() == "ACTIVE");
        }

        public async System.Threading.Tasks.Task AddUserAsync(User user) => await _context.Users.AddAsync(user);

        public async Task<bool> EmailExistsAsync(string email)
            => await _context.Users.AsNoTracking().AnyAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower());

        public void UpdateUser(User user) => _context.Users.Update(user);

        public void DeleteUser(User user)
        {
            user.Status = "INACTIVE";
            _context.Users.Update(user);
        }
        public async Task<IEnumerable<User>> GetUsersWithoutRoleAsync()
        {
            return await _context.Users
                .Where(u => u.RoleId == null && u.Status.ToUpper() == "ACTIVE")
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
