using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs
{
    public record PersonDto(
     int PersonID,
     string NationalID,
     string FirstName,
     string LastName,
     string Address,
     string Phone,
     string Email,
     int CountryID)
    {
        public string FullName => $"{FirstName} {LastName}";
    }

}
