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
    public class PersonService : IService<PersonDto, Person>
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
            IEnumerable<Person> People = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<PersonDto>>(People);
        }
        async public Task<PersonDto?> GetByID(int id)
        {
            var Person = await _repository.GetByIdAsync(id);

            return _mapper.Map<PersonDto>(Person);
        }
        async public Task<bool> AddNew(Person Entity)
        {
            return await _repository.Add(Entity);
        }
        async public Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        public async Task<bool> Update(Person Entity)
        {
            return await _repository.Update(Entity);
        }
    }
}
