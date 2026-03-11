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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {_dbContext = dbContext;}

        public async Task<bool> Add(User entity)
        {
            if (entity == null)
                return false;

            _dbContext.Users.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            User user = await _dbContext.Users.FindAsync(id);

            if (user == null) 
                return false;

            user.IsActive = false;

            return await Save();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<bool> Update(User entity)
        {
            if (entity == null)
                return false;

            User user = await _dbContext.Users.FindAsync(entity.UserID);

            if (user == null)
                return false;

            _dbContext.Entry(user).CurrentValues.SetValues(entity);
            
            user.PersonInfo = entity.PersonInfo;

            return await Save();
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
        }
        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
