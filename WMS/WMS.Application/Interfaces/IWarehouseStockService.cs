using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces
{
    public interface IWarehouseStockService : IService<WarehouseStockDto, WarehouseStock>
    {
        Task<decimal> GetTotalInventoryValue();
        public Task<bool> IsExistCombination(int warehouseId, int itemId);
        public  Task<bool> TransferStock(WarehouseTransferDto transferDto);



    }
}
