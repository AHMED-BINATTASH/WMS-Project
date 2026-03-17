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
    public class ItemUnitRepository : IItemUnitRepository
    {
        private readonly AppDbContext _dbContext;

        public ItemUnitRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(ItemUnit entity)
        {
            if (entity == null) return false;

            _dbContext.ItemUnits.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            ItemUnit itemUnit = await _dbContext.ItemUnits.FindAsync(id);

            if (itemUnit == null) return false;

            _dbContext.Remove(itemUnit);

            return await Save();
        }

        public async Task<IEnumerable<ItemUnit>> GetAllAsync()
        {
            return await _dbContext.ItemUnits.AsNoTracking().ToListAsync();
        }

        public async Task<ItemUnit> GetByIdAsync(int id)
        {
            return await _dbContext.ItemUnits.FindAsync(id);
        }

        public async Task<bool> Update(ItemUnit entity)
        {
            if (entity == null) return false;

            ItemUnit itemUnit = await _dbContext.ItemUnits.FindAsync(entity.ItemUnitID);

            if (itemUnit == null) return false;

            _dbContext.Entry(itemUnit).CurrentValues.SetValues(entity);

            itemUnit.ItemInfo = entity.ItemInfo;
            itemUnit.UnitInfo = entity.UnitInfo;

            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsExistCombinationAsync(int itemId, int unitId)
        {
            if (itemId < 1 || unitId < 1)
                return false;

            return await _dbContext.ItemUnits.AnyAsync(iu => iu.ItemID == itemId &&  iu.UnitID == unitId);
        }
    }
}
