using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    internal class WarehouseStockRepository : IRepository<WarehouseStock>
    {
        public Task<bool> Add(WarehouseStock entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WarehouseStock>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WarehouseStock> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(WarehouseStock entity)
        {
            throw new NotImplementedException();
        }
    }
}
