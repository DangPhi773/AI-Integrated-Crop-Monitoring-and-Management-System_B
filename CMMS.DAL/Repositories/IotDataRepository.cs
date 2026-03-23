using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMMS.DAL.Repositories
{
    public class IotDataRepository : IIotDataRepository
    {
        private readonly AppDbContext _context;
        public IotDataRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task<IEnumerable<IotData>> GetAllAsync()
        {
            return await _context.IotDatas
                .Include(d => d.Device)
                .Include(d => d.Season)
                .AsNoTracking()
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IotData?> GetByIdAsync(Guid id)
        {
            return await _context.IotDatas
                .Include(d => d.Device)
                .Include(d => d.Season)
                .FirstOrDefaultAsync(d => d.SensorDataId == id);
        }

        public async System.Threading.Tasks.Task<IEnumerable<IotData>> GetByDeviceIdAsync(Guid deviceId)
        {
            return await _context.IotDatas
                .Include(d => d.Device)
                .Include(d => d.Season)
                .Where(d => d.DeviceId == deviceId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(IotData entity) => await _context.IotDatas.AddAsync(entity);

        public void Delete(IotData entity) => _context.IotDatas.Remove(entity);

        public async System.Threading.Tasks.Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
