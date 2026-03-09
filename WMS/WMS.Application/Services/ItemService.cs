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
            if (Entity == null)
                return false;
            
            var isAdded = await _repository.Add(Entity);

            return isAdded;
        }

        public async Task<bool> Delete(int id)
        {
            var isDeleted = await _repository.Delete(id);
            return isDeleted;
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

      

     async   public Task<bool> Update(Item Entity)
        {
            if (Entity == null)
                return false;

            var existingItem = await _repository.GetByIdAsync(Entity.ItemID);

            if (existingItem == null)
                return false;

            existingItem.ItemName = Entity.ItemName;
            existingItem.Barcode = Entity.Barcode;
            existingItem.ReorderPoint=Entity.ReorderPoint;
            existingItem.AverageCost=Entity.AverageCost;
            existingItem.IsActive=Entity.IsActive;
            existingItem.IsExpiryRelated=Entity.IsExpiryRelated;
            existingItem.CreatedBy=Entity.CreatedBy;
            existingItem.CreatedAt=Entity.CreatedAt;


            return await _repository.Update(existingItem);


        }
    }
}