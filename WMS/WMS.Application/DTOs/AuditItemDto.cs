using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Application.DTOs
{
    public record AuditItemDto(
     int AuditItemID,
     int WarehouseId,
     Warehouse Warehouse, 
     int ItemId,
     Item Item,           
     string FieldName,
     string OldValue,
     string NewValue,
     string ActionType,
     DateTime CreatedAt,
     int CreatedBy,
     User CreatorInfo
 );
}
