using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{

    public class Person
    {
        public int PersonID { get; private set; }
        public string NationalID { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Foreign Key and Navigation Property
        public int CountryID { get; set; }
        public Country CountryInfo { get; set; }

        public Person(Country country, int personID, string nationalID)
        {
            CountryInfo = country;
            PersonID = personID;
            NationalID = nationalID;


        }
    }

}
