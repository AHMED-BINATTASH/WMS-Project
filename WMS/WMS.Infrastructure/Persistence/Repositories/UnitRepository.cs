using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    internal class UnitRepository : IRepository<Unit>
    {
        public Task<bool> Add(Unit entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Unit>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Unit> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Unit entity)
        {
            throw new NotImplementedException();
        }
    }
}
