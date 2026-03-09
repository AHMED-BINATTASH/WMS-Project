using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class UserService : IService<UserDto, User>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        async public Task<IEnumerable<UserDto>?> GetAll()
        {
            var People = await _repository.GetAllAsync();

            return People != null ? _mapper.Map<IEnumerable<UserDto>>(People) : null;
        }

        async public Task<UserDto?> GetByID(int id)
        {

            var User = await _repository.GetByIdAsync(id);

            return User != null ? _mapper.Map<UserDto>(User) : null;
        }

    

        async public Task<bool> AddNew(User Entity)
        {
            if (Entity == null)
                return false;

            var IsAdded = await _repository.Add(Entity);

            return IsAdded;
        }

        async public Task<bool> Delete(int id)
        {
            var IsDeleted = await _repository.Delete(id);
            return IsDeleted;

        }

        async public Task<bool> Update(User Entity)
        {
            if (Entity == null)
                return false;

            var existingUser = await _repository.GetByIdAsync(Entity.UserID);

            if (existingUser == null)
                return false;

            existingUser.Username = Entity.Username;
            existingUser.Password = Entity.Password;
            existingUser.IsActive = Entity.IsActive;
            existingUser.Role = Entity.Role;

            return await _repository.Update(existingUser);
        }
    }
}
