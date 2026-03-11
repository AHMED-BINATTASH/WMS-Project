using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CategoryRepository : IRepository<Category>
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Category entity)
        {
            if (entity == null)
                return false;

            _dbContext.Categories.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            Category category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
                return false;

            _dbContext.Remove(category);

            return await Save();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task<bool> Update(Category entity)
        {
            if (entity == null)
                return false;

            Category category = await _dbContext.Categories.FindAsync(entity.CategoryID);

            if (category == null)
                return false;

            _dbContext.Entry(category).CurrentValues.SetValues(entity);

            category.ParentCategoryInfo = entity.ParentCategoryInfo;

            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
