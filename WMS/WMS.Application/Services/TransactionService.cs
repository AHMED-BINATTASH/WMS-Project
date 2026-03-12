using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class TransactionService
    {

        private readonly ITransaction _transaction;
        public TransactionService(ITransaction transaction)
        {
            _transaction = transaction;
        }
        public void SaveTransaction()
        {
            _transaction.Save();
        }
    }
}
