using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Interfaces;

namespace WMS.Domain.Entities
{
    public class AuditItem : IAuditLog
    {
        public int WarehouseId { get; set; }
        public int ItemId { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ActionType { get; set; }

        private AuditItem() { }

        public AuditItem(int warehouseId, int itemId, string fieldName, string oldValue, string newValue, int createdBy, string actionType)
        {
            this.WarehouseId = warehouseId;
            this.ItemId = itemId;
            this.FieldName = fieldName;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.CreatedBy = createdBy;
            this.ActionType = actionType;
            this.CreatedAt = DateTime.Now;
        }

        public bool Save()
        {
            return true;
        }
    }
}
