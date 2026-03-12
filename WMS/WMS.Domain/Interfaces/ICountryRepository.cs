using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        public Task<bool> IsExistByCountryNameAsync(string username);
        public Task<bool> IsExistByCountryIDAsync(int countryID);
    }
}
