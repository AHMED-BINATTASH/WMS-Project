using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Interfaces;

namespace WMS.Domain.Entities
{
    public class SystemTransaction 
    {
        public int TransactionID { get; private set; }
        public int WarehouseID { get; private set; }
        public int ItemID { get; private set; }
        public int TransactionTypeID { get; private set; }
        public int CreatedBy { get; private set; }

        public Warehouse WarehouseInfo { get; set; }
        public Item ItemInfo { get; set; }
        public TransactionType TransactionTypeInfo { get; set; }
        public User CreatorInfo { get; set; }

        public int Quantity { get; set; }
        public decimal RunningBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }

        // This constructor for EF Core to can create instance from SystemTransaction class
        private SystemTransaction() { }

        public SystemTransaction( Warehouse warehouse, Item item, TransactionType type, User user)
        {
           
            this.ItemID = item.ItemID;
            this.WarehouseInfo = warehouse;
            this.WarehouseID = warehouse.WarehouseID;
            this.ItemInfo = item;
            this.TransactionTypeInfo = type;
            this.TransactionTypeID = type.TransactionTypeID;
            this.CreatorInfo = user;
            this.CreatedBy = user.UserID; 
            this.CreatedAt = DateTime.Now;
        }
    }
}
