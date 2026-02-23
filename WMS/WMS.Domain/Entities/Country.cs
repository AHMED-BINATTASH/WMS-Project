using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class Country
    {
        public int CountryID { get; private set; } // Matches diagram: int private set
        public string CountryName { get; set; }

        // Constructor as shown in diagram
        public Country(int countryID)
        {
            CountryID = countryID;
        }

      
    }
}
