using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Interfaces;

namespace WMS.Domain.Entities
{
    public class AuditItem
    {
        public int AuditItemID { get; private set; }
        public int WarehouseId { get; private set; }
        public Warehouse Warehouse { get; private set; }
        public int ItemId { get; private set; }
        public Item Item { get; private set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int CreatedBy { get; set; }
        public User CreatorInfo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ActionType { get; set; }        
 
        private AuditItem() { }

        public AuditItem(Item item, Warehouse warehouse,string fieldName, string oldValue, string newValue, int createdBy,
            string actionType)
        {
            Warehouse = warehouse;
            Item = item;
            this.WarehouseId = warehouse.WarehouseID;
            this.ItemId = item.ItemID;
            this.FieldName = fieldName;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.CreatedBy = createdBy;
            this.ActionType = actionType;
            this.CreatedAt = DateTime.Now;
        }
    }
}
