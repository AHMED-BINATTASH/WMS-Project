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
    internal class UserService : IService<UserDto>
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        public UserService(IRepository<User> repository, IMapper mapper)
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

        async public Task<bool> AddNew(UserDto Entity)
        {
            if (Entity == null)
                return false;

            var User = _mapper.Map<User>(Entity);
            var IsAdded = await _repository.Add(User);

            return IsAdded;
        }

        async public Task<bool> Delete(int id)
        {
            var IsDeleted = await _repository.Delete(id);
            return IsDeleted;

        }

        public Task<bool> Update()
        {
            throw new NotImplementedException();
        }

    }
}
