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
    public class PersonService : IService<PersonDto>
    {
        private readonly IRepository<Person> _repository;
        private readonly IMapper _mapper;
        public PersonService(IRepository<Person> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }



        async public Task<IEnumerable<PersonDto>?> GetAll()
        {
            var People = await _repository.GetAllAsync();

            return People != null ? _mapper.Map<IEnumerable<PersonDto>>(People) : null;
        }

        async public Task<PersonDto?> GetByID(int id)
        {

            var Person = await _repository.GetByIdAsync(id);

            return Person != null ? _mapper.Map<PersonDto>(Person) : null;
        }

        async public Task<bool> AddNew(PersonDto Entity)
        {
            if (Entity == null)
                return false;

            var person = _mapper.Map<Person>(Entity);
            var IsAdded = await _repository.Add(person);

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
