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
    public class WarehouseStockService : IService<WarehouseStockDto, WarehouseStock>
    {
        private readonly IRepository<WarehouseStock> _repository;
        private readonly IMapper _mapper;

        public WarehouseStockService(IRepository<WarehouseStock> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<bool> AddNew(WarehouseStock Entity)
        {
            return await _repository.Add(Entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<WarehouseStockDto>?> GetAll()
        {
            IEnumerable<WarehouseStock> warehouseStocks = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<WarehouseStockDto>>(warehouseStocks);
        }

        public async Task<WarehouseStockDto?> GetByID(int id)
        {
            var warehouseStock = await _repository.GetByIdAsync(id);
            return _mapper.Map<WarehouseStockDto>(warehouseStock);
        }

        public async Task<bool> Update(WarehouseStock Entity)
        {
            return await _repository.Update(Entity);
        }
    }
}
