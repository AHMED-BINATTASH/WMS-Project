using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly AppDbContext _dbContext;

        public PersonRepository(AppDbContext dbContext)
        {_dbContext = dbContext;}

        public async Task<bool> Add(Person entity)
        {
            if (entity == null)
                return false;

            _dbContext.People.Add(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbContext.People.FindAsync(id);

            if (entity == null)
                return false;

            _dbContext.People.Remove(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _dbContext.People.AsNoTracking().ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await _dbContext.People.FindAsync(id);
        }

        public async Task<bool> Update(Person entity)
        {
            if (entity == null)
                return false;

            Person person = await _dbContext.People.FindAsync(entity.PersonID);

            if (person == null)
                return false;

            _dbContext.Entry(person).CurrentValues.SetValues(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
