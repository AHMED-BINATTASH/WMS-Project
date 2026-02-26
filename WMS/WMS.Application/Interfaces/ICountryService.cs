using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Interfaces
{

    public interface ICountryService
    {
        // 
        Task<IEnumerable<CountryDto>> GetAvailableCountries();
    }

}
