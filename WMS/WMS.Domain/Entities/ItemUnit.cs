using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class ItemUnit
    {
        public int ItemUnitID { get; private set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public short? Factor { get; set; }

        // This constructor for EF Core to can create instance from ItemUnit class
        private ItemUnit() { }

        // Constructor 
        public ItemUnit(int itemUnitId)
        {
            this.ItemUnitID = itemUnitId;
        }
    }
}
