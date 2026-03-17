using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public record CategoryDto(
        int CategoryID,
        string CategoryName,
        string? Description,
        int? ParentCategoryID
    );
}



