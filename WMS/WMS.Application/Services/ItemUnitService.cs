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
    public class ItemUnitService : IItemUnitService
    {
        private readonly IItemUnitRepository _repository;
        private readonly IMapper _mapper;

        public ItemUnitService(IItemUnitRepository repository, IMapper mapper)
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
            IEnumerable<ItemUnit> itemUnits = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<ItemUnitDto>>(itemUnits);

        }

        public async Task<ItemUnitDto?> GetByID(int id)
        {
            ItemUnit itemUnit = await _repository.GetByIdAsync(id);

            return _mapper.Map<ItemUnitDto>(itemUnit);

        }

        async public Task<bool> Update(ItemUnit Entity)
        {
            return await _repository.Update(Entity);
        }
        public Task<bool> IsExistCombination(int itemId, int unitId)
        {
            throw new NotImplementedException();
        }

    }
}