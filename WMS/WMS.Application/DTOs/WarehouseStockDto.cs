using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Application.DTOs
{
    public record WarehouseStockDto(
     int WarehouseStockID,
     int WarehouseID,
     int ItemID,
     int Quantity,
     string BatchNumber,
     decimal ActualCost,
     DateTime ProductionDate,
     DateTime ExpiryDate,
     DateTime CreatedAt,
     int CreatedBy
 );


    public record AddWarehouseStockDto(
          int WarehouseID,
        int ItemID,
     int Quantity,
     string BatchNumber,
     decimal ActualCost,
     DateTime ProductionDate,
     DateTime ExpiryDate,
     DateTime CreatedAt,
     int CreatedBy);

}
