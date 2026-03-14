using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IWarehouseStockRepository : IRepository<WarehouseStock>
    {
        public Task<bool> IsExistCombination(int warehouseId, int itemId);
    }
}
