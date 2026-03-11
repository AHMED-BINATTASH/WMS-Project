using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Application.DTOs
{
    public record WarehouseDto(int WarehouseID,
       string WarehouseCode,
       string WarehouseName,
       string Location,
       bool IsActive
        );

}
