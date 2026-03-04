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
    public class UnitService : IService<UnitDto>
    {

        private readonly IRepository<Unit> _repository;
        private readonly IMapper _mapper;
        public UnitService(IRepository<Unit> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        async public Task<bool> AddNew(UnitDto Entity)
        {
            if (Entity == null)
                return false;

            var Unit = _mapper.Map<Unit>(Entity);

            bool IsAdded = await _repository.Add(Unit);

            return IsAdded;

        }

        public async Task<bool> Delete(int id)
        {
            bool IsDeleted = await _repository.Delete(id);
            return IsDeleted;
        }

        public async Task<IEnumerable<UnitDto>?> GetAll()
        {
            var Units = await _repository.GetAllAsync();
            return Units != null ? _mapper.Map<IEnumerable<UnitDto>>(Units) : null;
        }

        public async Task<UnitDto?> GetByID(int id)
        {
            var Unit = await _repository.GetByIdAsync(id);
            return Unit!=null? _mapper.Map<UnitDto>(Unit) : null;
        }

        public Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}
