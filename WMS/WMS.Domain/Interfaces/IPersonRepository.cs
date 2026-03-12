using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IPersonRepository : IRepository<Person>
    {
         Task<bool> IsExistByNationalIDAsync(string NationalID);
        Task<bool> IsExistByEmailIDAsync(string Email);
        Task<bool> IsExistByPersonIDAsync(int PersonID);
    }
}
