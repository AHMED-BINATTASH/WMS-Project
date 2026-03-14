using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Application.DTOs.WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        async public Task<bool> AddNew(Item Entity)
        {
            return await _repository.Add(Entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<ItemDto>?> GetAll()
        {
            IEnumerable<Item> items = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<ItemDto>>(items);

        }

        public async Task<ItemDto?> GetByID(int id)
        {
            Item item = await _repository.GetByIdAsync(id);

            return _mapper.Map<ItemDto>(item);

        }


        public async Task<bool> Update(Item Entity)
        {
            return await _repository.Update(Entity);
        }
        public Task<bool> IsExistByName(string itemName)
        {
            return _repository.IsExistByName(itemName);
        }
    }
}