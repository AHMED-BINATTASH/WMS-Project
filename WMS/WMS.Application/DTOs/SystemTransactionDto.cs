using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Application.DTOs
{
    public record SystemTransactionDto(
     int TransactionID,
     int WarehouseID,
     Warehouse WarehouseInfo, 
     int ItemID,
     Item ItemInfo,
     int TransactionTypeID,
     TransactionType TransactionTypeInfo,
     int Quantity,
     decimal RunningBalance,
     string Description,
     string ReferenceNumber,
     DateTime CreatedAt,
     int CreatedBy,
     User CreatorInfo
 );
}
