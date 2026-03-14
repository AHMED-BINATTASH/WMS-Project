using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces
{
    public interface IItemUnitService : IService<ItemUnitDto, ItemUnit>
    {
        public Task<bool> IsExistCombination(int itemId,int unitId);
    }
}
