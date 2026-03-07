using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class Warehouse
    {
       
        public int WarehouseID { get; private set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }

        // This constructor for EF Core to can create instance from Warehouse class
        private Warehouse() { }
       
        public Warehouse(int warehouseID)
        {
            WarehouseID = warehouseID;
        }
    }
}
