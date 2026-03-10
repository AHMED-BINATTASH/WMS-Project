using AutoMapper;
using Microsoft.VisualBasic;
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
    public class WarehouseService : IService<WarehouseDto, Warehouse>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Warehouse> _repository;

        public WarehouseService(IRepository<Warehouse> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<bool> AddNew(Warehouse Entity)
        {
            return await _repository.Add(Entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<WarehouseDto>?> GetAll()
        {
            var Warehouses = await _repository.GetAllAsync();
            return Warehouses != null ? _mapper.Map<IEnumerable<WarehouseDto>>(Warehouses) : null;

        }

        public async Task<WarehouseDto?> GetByID(int id)
        {
            var warehouse = await _repository.GetByIdAsync(id);
            return warehouse != null ? _mapper.Map<WarehouseDto>(warehouse) : null;
        }

        public async Task<bool> Update(Warehouse Entity)
        {
            return await _repository.Update(Entity);
        }
    }
}
