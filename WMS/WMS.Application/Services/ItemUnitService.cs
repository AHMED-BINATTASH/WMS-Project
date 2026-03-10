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
    public class ItemUnitService : IService<ItemUnitDto, ItemUnit>
    {
        private readonly IRepository<ItemUnit> _repository;
        private readonly IMapper _mapper;

        public ItemUnitService(IRepository<ItemUnit> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> AddNew(ItemUnit entity)
        {
            return await _repository.Add(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
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

        async public Task<bool> Update(ItemUnit Entity)
        {
            return await _repository.Update(Entity);
        }
    }
}