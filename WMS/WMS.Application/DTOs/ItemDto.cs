using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Application.DTOs
{
    namespace WMS.Application.DTOs
    {
        public record ItemDto
        (
            int ItemID,
            string ItemName,
            string Barcode,
            int UnitID,
            int CategoryID,
            decimal ReorderPoint,
            decimal AverageCost,
            bool IsActive,
            bool IsExpiryRelated,
            DateTime CreatedAt,
            int CreatedBy
        );
    }
}
