using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    internal interface IRepoaitory<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task Add(T entity);
        Task Delete(int id);
        Task Update();
    }
}
