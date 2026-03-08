using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    internal class WarehouseRepository : IRepository<Warehouse>
    {
        public Task<bool> Add(Warehouse entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Warehouse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Warehouse entity)
        {
            throw new NotImplementedException();
        }
    }
}
