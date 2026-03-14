using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces
{
    public interface IUnitService : IService<UnitDto,Unit>
    {
        public Task<bool> IsExistByName(string unitName);
    }
}
