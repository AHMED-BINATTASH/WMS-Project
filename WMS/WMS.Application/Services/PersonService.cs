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
            var People = await _repository.GetAllAsync();

            return People != null ? _mapper.Map<IEnumerable<PersonDto>>(People) : null;
        }

        async public Task<PersonDto?> GetByID(int id)
        {

            var Person = await _repository.GetByIdAsync(id);

            return Person != null ? _mapper.Map<PersonDto>(Person) : null;
        }

        async public Task<bool> AddNew(Person Entity)
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

        //public Task<bool> Update(Person entity)
        //{
        //    return _repository.Update(entity);
        //}

        async public Task<bool> Update(Person Entity)
        {
            if (Entity == null)
                return false;
            var existingPerson = await _repository.GetByIdAsync(Entity.PersonID);

            if (existingPerson == null)
                return false;

            existingPerson.FirstName = Entity.FirstName;
            existingPerson.LastName = Entity.LastName;
            existingPerson.Email = Entity.Email;
            existingPerson.Phone = Entity.Phone;
            existingPerson.Address = Entity.Address;
            existingPerson.CountryID = Entity.CountryID;


            return await _repository.Update(existingPerson);
        }

        public async Task<bool> IsExistByNationalID(string NationalID)
        {
            return await _repository.IsExistByNationalIDAsync(NationalID);
        }

        public async Task<bool> IsExistByEmail(string Email)
        {
            return await _repository.IsExistByEmailIDAsync(Email);

        }
    }
}
