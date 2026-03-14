using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly AppDbContext _dbContext;

        public UnitRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Unit entity)
        {
            if (entity == null) return false;

            _dbContext.Units.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            Unit unit = await _dbContext.Units.FindAsync(id);

            if (unit == null) return false;

            _dbContext.Remove(unit);

            return await Save();
        }

        public async Task<IEnumerable<Unit>> GetAllAsync()
        {
            return await _dbContext.Units.AsNoTracking().ToListAsync();
        }

        public async Task<Unit> GetByIdAsync(int id)
        {
            return await _dbContext.Units.FindAsync(id);
        }

        public async Task<bool> Update(Unit entity)
        {
            if (entity == null) return false;

            Unit unit = await _dbContext.Units.FindAsync(entity.UnitID);

            if (unit == null) return false;

            _dbContext.Entry(unit).CurrentValues.SetValues(entity);

            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsExistByName(string unitName)
        {
            if (string.IsNullOrEmpty(unitName))
                return false;

            return await _dbContext.Units.AnyAsync(u => u.UnitName == unitName);
        }
    }
}
