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
    public class UnitService : IService<UnitDto,Unit>
    {

        private readonly IRepository<Unit> _repository;
        private readonly IMapper _mapper;
        public UnitService(IRepository<Unit> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        async public Task<bool> AddNew(Unit Entity)
        {
            return await _repository.Add(Entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<UnitDto>?> GetAll()
        {
            IEnumerable<Unit> Units = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<UnitDto>>(Units);
        }

        public async Task<UnitDto?> GetByID(int id)
        {
            Unit Unit = await _repository.GetByIdAsync(id);
            return _mapper.Map<UnitDto>(Unit);
        }

        public async Task<bool> Update(Unit Entity)
        {
            return await _repository.Update(Entity);
        }
    }
}
