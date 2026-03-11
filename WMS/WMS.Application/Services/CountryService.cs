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
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;
        public CountryService(ICountryRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        async public Task<IEnumerable<CountryDto>?> GetAll()
        {
            IEnumerable<Country> countries = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<CountryDto>>(countries);
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
        public Task<bool> IsExistByCountryName(string CountryName)
        {
            return _repository.IsExistByCountryNameAsync(CountryName);
        }
    }
}