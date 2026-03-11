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
    public class TransactionTypeRepository : IRepository<TransactionType>
    {
        private readonly AppDbContext _dbContext;

        public TransactionTypeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(TransactionType entity)
        {
            if (entity == null) return false;

            _dbContext.TransactionTypes.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            TransactionType type = await _dbContext.TransactionTypes.FindAsync(id);

            if (type == null) return false;

            _dbContext.Remove(type);

            return await Save();
        }

        public async Task<IEnumerable<TransactionType>> GetAllAsync()
        {
            return await _dbContext.TransactionTypes.AsNoTracking().ToListAsync();
        }

        public async Task<TransactionType> GetByIdAsync(int id)
        {
            return await _dbContext.TransactionTypes.FindAsync(id);
        }

        public async Task<bool> Update(TransactionType entity)
        {
            if (entity == null) return false;

            TransactionType type = await _dbContext.TransactionTypes.FindAsync(entity.TransactionTypeID);

            if (type == null) return false;

            _dbContext.Entry(type).CurrentValues.SetValues(entity);

            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
