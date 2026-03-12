using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class SystemTransactionService
    {
        private readonly IRepository<SystemTransaction> _repository;

        public SystemTransactionService(IRepository<SystemTransaction> repository)
        {
            _repository = repository;
        }
       
    }
}
