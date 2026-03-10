using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Application.DTOs.WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class ItemService : IService<ItemDto,Item>
    {
        private readonly IRepository<Item> _repository;
        private readonly IMapper _mapper;

        public ItemService(IRepository<Item> repository, IMapper mapper)
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
            var items = await _repository.GetAllAsync();

            return items != null
                ? _mapper.Map<IEnumerable<ItemDto>>(items)
                : null;
        }

        public async Task<ItemDto?> GetByID(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            return item != null
                ? _mapper.Map<ItemDto>(item)
                : null;
        }
        public async Task<bool> Update(Item Entity)
        {
            return await _repository.Update(Entity);
        }
    }
}