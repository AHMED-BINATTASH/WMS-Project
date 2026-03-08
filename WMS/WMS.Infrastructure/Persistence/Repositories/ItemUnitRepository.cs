using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    internal class ItemUnitRepository : IRepository<ItemUnit>
    {
        public Task<bool> Add(ItemUnit entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ItemUnit>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ItemUnit> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ItemUnit entity)
        {
            throw new NotImplementedException();
        }
    }
}
