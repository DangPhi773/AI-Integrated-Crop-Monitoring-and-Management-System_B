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
    public class PlotRepository : IPlotRepository
    {
        private readonly AppDbContext _context;
        public PlotRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Plot>> GetAllAsync()
            => await _context.Plots
                .Include(p => p.Farm)
                .Include(p => p.Soil)
                .Include(p => p.Beds)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Plot?> GetByIdAsync(Guid id)
            => await _context.Plots
                .Include(p => p.Farm)
                .Include(p => p.Soil)
                .Include(p => p.Beds)
                .FirstOrDefaultAsync(p => p.PlotId == id);

        public async System.Threading.Tasks.Task AddAsync(Plot plot) => await _context.Plots.AddAsync(plot);

        public void Update(Plot plot) => _context.Plots.Update(plot);

        public void Delete(Plot plot) => _context.Plots.Remove(plot);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
