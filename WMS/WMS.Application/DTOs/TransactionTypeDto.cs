using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs
{
    public record TransactionTypeDto
     (
        int TransactionTypeID,
        string TransactionTypeName,
      string Description);
}
