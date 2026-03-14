using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IUnitRepository : IRepository<Unit>
    {
        public Task<bool> IsExistByName(string unitName);
    }
}
