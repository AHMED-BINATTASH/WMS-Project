using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class Country
    {
        public int CountryID { get; private set; } 
        public string CountryName { get; set; }

        // This constructor for EF Core to can create instance from Country class
        private Country() { }

        public Country(int countryID)
        {
            CountryID = countryID;
        }

      
    }
}
