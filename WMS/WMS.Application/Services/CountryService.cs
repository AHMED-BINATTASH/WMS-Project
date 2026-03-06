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
    public class CountryService : IService<CountryDto>
    {
        private readonly IRepository<Country> _repository;
        private readonly IMapper _mapper;
        public CountryService(IRepository<Country> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }



        async public Task<IEnumerable<CountryDto>?> GetAll()
        {
            var People = await _repository.GetAllAsync();

            return People != null ? _mapper.Map<IEnumerable<CountryDto>>(People) : null;
        }

        async public Task<CountryDto?> GetByID(int id)
        {

            var Country = await _repository.GetByIdAsync(id);

            return Country != null ? _mapper.Map<CountryDto>(Country) : null;
        }

        async public Task<bool> AddNew(CountryDto Entity)
        {
            if (Entity == null)
                return false;

            var Country = _mapper.Map<Country>(Entity);
            var IsAdded = await _repository.Add(Country);

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