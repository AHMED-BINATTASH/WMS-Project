using AutoMapper;
using System;
using System.Collections;
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
    public class UserService : IUserService
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
            IEnumerable<User> users= await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        async public Task<UserDto?> GetByID(int id)
        {
            User User = await _repository.GetByIdAsync(id);

            return _mapper.Map<UserDto>(User);
        }
        public async Task<User> GetByUsername(string username)
        {
            return await _repository.GetByUsernameAsync(username);
        }
        async public Task<bool> AddNew(User Entity)
        {
            return await _repository.Add(Entity);
        }
        async public Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        async public Task<bool> Update(User Entity)
        {
            return await _repository.Update(Entity);
        }
    }
}
