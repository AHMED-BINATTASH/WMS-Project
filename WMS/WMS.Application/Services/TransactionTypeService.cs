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
    public class TransactionTypeService : IService<TransactionTypeDto, TransactionType>
    {
        private readonly IRepository<TransactionType> _repository;
        private readonly IMapper _mapper;
        public TransactionTypeService(IRepository<TransactionType> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<bool> AddNew(TransactionType Entity)
        {
            return await _repository.Add(Entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<TransactionTypeDto>?> GetAll()
        {
            var TransactionTypes = await _repository.GetAllAsync();

            return TransactionTypes != null ? _mapper.Map<IEnumerable<TransactionTypeDto>>(TransactionTypes) : null;

        }

        public async Task<TransactionTypeDto?> GetByID(int id)
        {
            var TransactionType = await _repository.GetByIdAsync(id);

            return TransactionType != null ? _mapper.Map<TransactionTypeDto>(TransactionType) : null;
        }

        public Task<bool> Update(TransactionType Entity)
        {
            return _repository.Update(Entity);
        }
    }
}
