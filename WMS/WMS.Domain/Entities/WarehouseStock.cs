using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class WarehouseStock
    {
        public int WarehouseStockID { get; private set; }
        public int WarehouseID { get; private set; }
        public int ItemID { get; private set; }
        public int CreatedBy { get; set; }
        public User CreatorInfo { get; set; }
        public Warehouse WarehouseInfo { get; set; }
        public Item ItemInfo { get; set; } 
        public int Quantity { get; set; }
        public string BatchNumber { get; set; }
        public decimal ActualCost { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }

        // This constructor for EF Core to can create instance from  WarehouseStock class
        private WarehouseStock() { }

        public WarehouseStock(Warehouse warehouse, Item item,User creatorInfo)
        {
            this.WarehouseInfo = warehouse;
            this.WarehouseID = warehouse.WarehouseID;
            this.ItemInfo = item;
            this.ItemID = item.ItemID;
            this.CreatorInfo = creatorInfo;
        }
    }
}
