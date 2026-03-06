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
    public class ItemUnitService : IService<ItemUnitDto>
    {
        private readonly IRepository<ItemUnit> _repository;
        private readonly IMapper _mapper;

        public ItemUnitService(IRepository<ItemUnit> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> AddNew(ItemUnitDto entity)
        {
            if (entity == null)
                return false;

            var itemUnit = _mapper.Map<ItemUnit>(entity);
            var isAdded = await _repository.Add(itemUnit);

            return isAdded;
        }

        public async Task<bool> Delete(int id)
        {
            var isDeleted = await _repository.Delete(id);
            return isDeleted;
        }

        public async Task<IEnumerable<ItemUnitDto>?> GetAll()
        {
            var itemUnits = await _repository.GetAllAsync();

            return itemUnits != null
                ? _mapper.Map<IEnumerable<ItemUnitDto>>(itemUnits)
                : null;
        }

        public async Task<ItemUnitDto?> GetByID(int id)
        {
            var itemUnit = await _repository.GetByIdAsync(id);

            return itemUnit != null
                ? _mapper.Map<ItemUnitDto>(itemUnit)
                : null;
        }

        public Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}