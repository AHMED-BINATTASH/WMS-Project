using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>?> GetAll();
        Task<T?> GetByID(int id);
        Task<bool> AddNew(T Entity);
        Task<bool> Delete(int id);
        Task<bool> Update(T entity);
    }
}
