using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CMMS.DAL.Repositories
{
    public class IotDeviceRepository : IIotDeviceRepository
    {
        private readonly AppDbContext _context;
        public IotDeviceRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task<IEnumerable<IotDevice>> GetAllAsync()
        {
            return await _context.IotDevices
                .Include(d => d.Bed)
                .AsNoTracking()
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IotDevice?> GetByIdAsync(Guid id)
        {
            return await _context.IotDevices
                .Include(d => d.Bed)
                .FirstOrDefaultAsync(d => d.DeviceId == id);
        }

        public async System.Threading.Tasks.Task<IotDevice?> GetByDeviceCodeAsync(string deviceCode)
        {
            return await _context.IotDevices
                .Include(d => d.Bed)
                    .ThenInclude(b => b!.Plot)
                .FirstOrDefaultAsync(d => d.DeviceCode == deviceCode);
        }

        public async System.Threading.Tasks.Task AddAsync(IotDevice entity) => await _context.IotDevices.AddAsync(entity);

        public void Update(IotDevice entity) => _context.IotDevices.Update(entity);

        public void Delete(IotDevice entity) => _context.IotDevices.Remove(entity);

        public async System.Threading.Tasks.Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
