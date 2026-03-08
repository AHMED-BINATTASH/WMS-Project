using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    internal class SystemTransactionRepository : IRepository<SystemTransaction>
    {
        public Task<bool> Add(SystemTransaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SystemTransaction>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SystemTransaction> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(SystemTransaction entity)
        {
            throw new NotImplementedException();
        }
    }
}
