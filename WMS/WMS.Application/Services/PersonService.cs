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
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;
        public PersonService(IPersonRepository repository, IMapper mapper)
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

        public async Task<bool> IsExistByNationalID(string NationalID)
        {
            return await _repository.IsExistByNationalIDAsync(NationalID);
        }

        public async Task<bool> IsExistByEmail(string Email)
        {
            return await _repository.IsExistByEmailIDAsync(Email);
        }

        public async Task<bool> IsExistByPersonID(int PersonID)
        {
            return await _repository.IsExistByPersonIDAsync(PersonID);
        }
    }
}
