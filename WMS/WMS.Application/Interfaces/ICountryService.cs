using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces
{
    public interface ICountryService : IService<CountryDto,Country>
    {
        Task<bool> IsExistByCountryName(string CountryName);
        Task<bool> IsExistByCountryID(int CountryID);
    }
}
