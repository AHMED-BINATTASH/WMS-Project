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
    public class WarehouseStockService : IService<WarehouseStockDto, Warehouse>
    {
        private readonly IRepository<Warehouse> _repository;
        private readonly IMapper _mapper;

        public WarehouseStockService(IRepository<Warehouse> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<bool> AddNew(Warehouse Entity)
        {
            return await _repository.Add(Entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<WarehouseStockDto>?> GetAll()
        {
            var warehouseStocks = await _repository.GetAllAsync();
            return warehouseStocks != null ? _mapper.Map<IEnumerable<WarehouseStockDto>>(warehouseStocks) : null;
        }

        public async Task<WarehouseStockDto?> GetByID(int id)
        {
            var warehouseStock = await _repository.GetByIdAsync(id);
            return warehouseStock != null ? _mapper.Map<WarehouseStockDto>(warehouseStock) : null;
        }

        public async Task<bool> Update(Warehouse Entity)
        {
            return await _repository.Update(Entity);
        }
    }
}
