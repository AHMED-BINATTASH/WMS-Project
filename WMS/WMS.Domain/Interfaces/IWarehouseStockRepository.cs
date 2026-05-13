using Microsoft.EntityFrameworkCore.Storage;
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
        // Check if the combination of warehouse and item exists
        Task<bool> IsExistCombination(int warehouseId, int itemId);

        Task<WarehouseStock> GetStockByWarehouseAndItem(int warehouseId, int itemId);

        // --- Transaction Management ---

        /// <summary>
        /// Starts a new database transaction.
        /// </summary>
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// Persists all tracked changes to the database.
        /// </summary>
        Task SaveChangesAsync();


        Task<decimal> GetTotalInventoryValue();
    }
}
