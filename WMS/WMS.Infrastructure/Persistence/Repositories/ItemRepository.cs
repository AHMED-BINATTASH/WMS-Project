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
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _dbContext;

        public ItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Item entity)
        {
            if (entity == null)
                return false;

            _dbContext.Items.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            Item item = await _dbContext.Items.FindAsync(id);

            if (item == null)
                return false;

            item.IsActive = false;

            return await Save();
        }
        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _dbContext.Items.AsNoTracking().ToListAsync();
        }

        public async Task<Item> GetByIdAsync(int id)
        {
            return await _dbContext.Items.FindAsync(id);
        }

        public async Task<bool> Update(Item entity)
        {
            if (entity == null)
                return false;

            Item item = await _dbContext.Items.FindAsync(entity.ItemID);

            if (item == null)
                return false;

            _dbContext.Entry(item).CurrentValues.SetValues(entity);

            item.CategoryInfo = entity.CategoryInfo;
            item.UnitInfo = entity.UnitInfo;
            item.CategoryInfo = entity.CategoryInfo;

            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsExistByName(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
                return false;

            return await _dbContext.Items.AnyAsync(i => i.ItemName == itemName);
        }
    }
}
