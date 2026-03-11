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
    public class SystemTransactionRepository : IRepository<SystemTransaction>
    {
        private readonly AppDbContext _dbContext;

        public SystemTransactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(SystemTransaction entity)
        {
            if (entity == null) return false;

            _dbContext.SystemTransactions.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            SystemTransaction transaction = await _dbContext.SystemTransactions.FindAsync(id);

            if (transaction == null) return false;
            
            _dbContext.Remove(transaction);

            return await Save();
        }

        public async Task<IEnumerable<SystemTransaction>> GetAllAsync()
        {
            return await _dbContext.SystemTransactions.AsNoTracking().ToListAsync();
        }

        public async Task<SystemTransaction> GetByIdAsync(int id)
        {
            return await _dbContext.SystemTransactions.FindAsync(id);
        }

        public async Task<bool> Update(SystemTransaction entity)
        {
            if (entity == null) return false;

            SystemTransaction transaction = await _dbContext.SystemTransactions.FindAsync(entity.TransactionID);

            if (transaction == null) return false;

            _dbContext.Entry(transaction).CurrentValues.SetValues(entity);

            transaction.WarehouseInfo = entity.WarehouseInfo;
            transaction.ItemInfo = entity.ItemInfo;
            transaction.TransactionTypeInfo = entity.TransactionTypeInfo;
            transaction.CreatorInfo = entity.CreatorInfo;

            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
