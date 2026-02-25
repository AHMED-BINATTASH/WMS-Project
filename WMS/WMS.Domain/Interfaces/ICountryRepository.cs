using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Interfaces
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country> GetByIdAsync(int id);
    }
}
