using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces
{
    public interface IPersonService : IService<PersonDto,Person>
    {
        Task<bool> IsExistByNationalID(string PersonName);
        Task<bool> IsExistByEmail(string PersonName);
    }
}
