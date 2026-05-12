using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    public class WarehouseStockRepository : IWarehouseStockRepository
    {
        private readonly AppDbContext _dbContext;

        public WarehouseStockRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(WarehouseStock entity)
        {
            if (entity == null) return false;

            _dbContext.WarehouseStocks.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            WarehouseStock stock = await _dbContext.WarehouseStocks.FindAsync(id);

            if (stock == null) return false;

            stock.IsActive = false;

            return await Save();
        }

        public async Task<IEnumerable<WarehouseStock>> GetAllAsync()
        {
            return await _dbContext.WarehouseStocks.AsNoTracking().ToListAsync();
        }

        public async Task<WarehouseStock> GetByIdAsync(int id)
        {
            return await _dbContext.WarehouseStocks.FindAsync(id);
        }

        public async Task<bool> Update(WarehouseStock entity)
        {
            if (entity == null) return false;
            WarehouseStock stock = await _dbContext.WarehouseStocks.FindAsync(entity.WarehouseStockID);
            if (stock == null) return false;

            _dbContext.Entry(stock).CurrentValues.SetValues(entity);

            stock.WarehouseInfo = entity.WarehouseInfo;
            stock.ItemInfo = entity.ItemInfo;
            stock.CreatorInfo = entity.CreatorInfo;

            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsExistCombination(int warehouseId, int itemId)
        {
            if (itemId < 1 || itemId < 1)
                return false;

            return await _dbContext.WarehouseStocks.AnyAsync(ws => ws.WarehouseID == warehouseId && ws.ItemID == itemId);
        }

       
      
     

        public async Task<WarehouseStock?> GetStockByWarehouseAndItem(int warehouseId, int itemId)
        {
            return await _dbContext.WarehouseStocks
                .FirstOrDefaultAsync(s => s.WarehouseID == warehouseId && s.ItemID == itemId);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
