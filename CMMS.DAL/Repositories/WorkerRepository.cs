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
    public class WorkerRepository : IWorkerRepository
    {
        private readonly AppDbContext _context;
        public WorkerRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName.ToLower().Trim() == roleName.ToLower().Trim());
        }

        public async System.Threading.Tasks.Task<IEnumerable<User>> GetAllWorkersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role != null &&
                       u.Role.RoleName.ToLower().Trim() == "worker")
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<User?> GetWorkerByIdAsync(Guid id)
        {
            return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == id &&
                                  u.Role != null &&
                                  u.Role.RoleName.ToLower().Trim() == "worker");
        }

        public async System.Threading.Tasks.Task AddWorkerAsync(User worker) => await _context.Users.AddAsync(worker);

        public async System.Threading.Tasks.Task<bool> EmailExistsAsync(string email)
            => await _context.Users.AsNoTracking().AnyAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower());

        public void UpdateWorker(User worker) => _context.Users.Update(worker);

        public void DeleteWorker(User worker) => _context.Users.Remove(worker);

        public async System.Threading.Tasks.Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
