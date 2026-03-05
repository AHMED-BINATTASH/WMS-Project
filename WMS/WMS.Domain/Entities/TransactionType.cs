using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class TransactionType
    {
        public int TransactionTypeID { get; private set; }
        public string TransactionTypeName { get; set; }
        public string Description { get; set; }

        // This constructor for EF Core to can create instance from TransactionType class
        private TransactionType() { }

        public TransactionType(int transactionTypeID)
        {
            this.TransactionTypeID = transactionTypeID;
        }
    }
}
