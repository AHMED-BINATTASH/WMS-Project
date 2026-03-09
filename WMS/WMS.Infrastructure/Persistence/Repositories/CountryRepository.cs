using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Persistence.Repositories
{
    public class CountryRepository : IRepository<Country>
    {
        private readonly AppDbContext _dbContext;

        public CountryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Country entity)
        {
            if (entity == null)
                return false;

            _dbContext.Countries.Add(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbContext.Countries.FindAsync(id);

            if (entity == null)
                return false;

            _dbContext.Countries.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await _dbContext.Countries.AsNoTracking().ToListAsync();
        }

        public async Task<Country> GetByIdAsync(int id)
        {
            return await _dbContext.Countries.FindAsync(id);
        }

        public async Task<bool> Update(Country entity)
        {
            if (entity == null)
                return false;

            Country country = await _dbContext.Countries.FindAsync(entity.CountryID);

            if (country == null)
                return false;

            _dbContext.Entry(country).CurrentValues.SetValues(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
