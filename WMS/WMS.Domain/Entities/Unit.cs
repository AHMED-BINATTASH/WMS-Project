
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class Unit
    {
        public int UnitID { get; private set; }
        public string UnitName { get; set; }
        public string UnitSymbol { get; set; }

        // This constructor for EF Core to can create instance from Unit class
        private Unit() { }

        // Constructor
        public Unit(int unitId)
        {
            this.UnitID = unitId;
        }
    }
}
