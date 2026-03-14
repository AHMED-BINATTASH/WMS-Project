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
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly AppDbContext _dbContext;

        public WarehouseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Warehouse entity)
        {
            if (entity == null) return false;

            _dbContext.Warehouses.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            Warehouse warehouse = await _dbContext.Warehouses.FindAsync(id);

            if (warehouse == null) return false;

            warehouse.IsActive = false;

            return await Save();
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            return await _dbContext.Warehouses.AsNoTracking().ToListAsync();
        }

        public async Task<Warehouse> GetByIdAsync(int id)
        {
            return await _dbContext.Warehouses.FindAsync(id);
        }

        public async Task<bool> Update(Warehouse entity)
        {
            if (entity == null) return false;

            Warehouse warehouse = await _dbContext.Warehouses.FindAsync(entity.WarehouseID);

            if (warehouse == null) return false;

            _dbContext.Entry(warehouse).CurrentValues.SetValues(entity);

            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsExistByName(string warehouseName)
        {
            if (string.IsNullOrEmpty(warehouseName))
                return false;

            return await _dbContext.Warehouses.AnyAsync(w => w.WarehouseName == warehouseName);
        }
    }
}
