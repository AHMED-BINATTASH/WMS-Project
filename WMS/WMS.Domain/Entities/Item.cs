using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class Item
    {
        // Properties
        public int ItemID { get; private set; }
        public string ItemName { get; set; }
        public string Barcode { get; set; }
        public int UnitID { get; private set; }
        public Unit UnitInfo { get; set; }
        public int CategoryID { get; private set; }
        public Category CategoryInfo { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal AverageCost { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpiryRelated { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        // This constructor for EF Core to can create instance from Item class
        private Item() { }

        // Constructor
        public Item(Category category, Unit unit, int itemID)
        {
            this.CategoryInfo = category;
            this.CategoryID = category.CategoryID;
            this.UnitInfo = unit;
            this.UnitID = unit.UnitID;
            this.ItemID = itemID;
            this.IsActive = true;
            this.CreatedAt = DateTime.Now;
        }

        // Methods
        public void SoftDelete()
        {
            this.IsActive = false;
        }
    }
}