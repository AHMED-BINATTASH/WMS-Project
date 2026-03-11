using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Interfaces
{
    public interface IService<TDto , TEntity> 
        where TDto : class 
        where TEntity : class
    {
        Task<IEnumerable<TDto>?> GetAll();
        Task<TDto?> GetByID(int id);
        Task<bool> AddNew(TEntity Entity);
        Task<bool> Delete(int id);
        Task<bool> Update(TEntity Entity);
        //Task<bool> IsExistByID(int id);
    }
}
