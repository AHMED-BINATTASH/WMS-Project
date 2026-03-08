using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    internal class TransactionTypeRepository : IRepository<TransactionType>
    {
        public Task<bool> Add(TransactionType entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TransactionType>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TransactionType> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(TransactionType entity)
        {
            throw new NotImplementedException();
        }
    }
}
