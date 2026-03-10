using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class TransactionService : IService<SystemTransactionDto, SystemTransaction>
    {
       
        private readonly ITransaction _transaction;
        private readonly IRepository<SystemTransaction> _repository;
        private readonly IMapper _mapper;

        public TransactionService(IRepository<SystemTransaction> repository, IMapper mapper, ITransaction transaction)
        {
            _mapper = mapper;
            _repository = repository;
            _transaction = transaction;
        }

        public async Task<bool> AddNew(SystemTransaction Entity)
        {
            return await _repository.Add(Entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<SystemTransactionDto>?> GetAll()
        {
           IEnumerable< SystemTransaction> transactions =await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<SystemTransactionDto>>(transactions);

        }

        public async Task<SystemTransactionDto?> GetByID(int id)
        {
            SystemTransaction transaction  = await _repository.GetByIdAsync(id);
            return _mapper.Map<SystemTransactionDto>(transaction);
        }

        public async Task<bool> Update(SystemTransaction Entity)
        {
            return await _repository.Update(Entity);
        }

        public void SaveTransaction()
        {
            _transaction.Save();
        }
    }
}
