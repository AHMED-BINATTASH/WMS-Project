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
    internal class AuditItemRepository : IRepository<AuditItem>
    {
        private readonly AppDbContext _dbContext;

        public AuditItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(AuditItem entity)
        {
            if (entity == null)
                return false;

            _dbContext.AuditItems.Add(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var auditItem = await _dbContext.AuditItems.FindAsync(id);

            if (auditItem == null)
                return false;

            _dbContext.AuditItems.Remove(auditItem);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<AuditItem>> GetAllAsync()
        {
            List<AuditItem> auditItems = await _dbContext.AuditItems
                .AsNoTracking()
                .ToListAsync<AuditItem>();

            return auditItems;  
        }

        public async Task<AuditItem> GetByIdAsync(int id)
        {
            AuditItem auditItem = await _dbContext.AuditItems.FindAsync(id);

            if (auditItem == null)
                return null;

            return auditItem;

        }

        public async Task<bool> Update(AuditItem entity)
        {
            if(entity == null)
                return false;

            AuditItem auditItem = await _dbContext.AuditItems.FindAsync(entity.AuditItemID);

            if (auditItem == null)
                return false;

            _dbContext.Entry(auditItem).CurrentValues.SetValues(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
