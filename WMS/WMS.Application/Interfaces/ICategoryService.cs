using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces
{
    public interface ICategoryService : IService<CategoryDto, Category>
    {
        public Task<bool> IsExistByName(string categoryName);
    }
}
