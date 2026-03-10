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
    public class CountryService : IService<CountryDto, Country>
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
            var countries = await _repository.GetAllAsync();

            return countries != null ? _mapper.Map<IEnumerable<CountryDto>>(countries) : null;
        }

        async public Task<CountryDto?> GetByID(int id)
        {
            Country country = await _repository.GetByIdAsync(id);

            return country != null ? _mapper.Map<CountryDto>(country) : null;
        }

        async public Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        async public Task<bool> AddNew(Country Entity)
        {
            return await _repository.Add(Entity); ;
        }

        async public Task<bool> Update(Country Entity)
        {
            return await _repository.Update(Entity);
        }
    }
}