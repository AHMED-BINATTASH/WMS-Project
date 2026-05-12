using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs
{
    public class WarehouseTransferDto
    {
        public int FromWarehouseID { get; set; }
        public int ToWarehouseID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public string? BatchNumber { get; set; } // Optional: if you track specific batches

        public int CreatedBy { get; set; }
    }
}
