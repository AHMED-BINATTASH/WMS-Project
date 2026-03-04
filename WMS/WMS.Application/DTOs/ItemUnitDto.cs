using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs
{
    public record ItemUnitDto(
        int ItemUnitID,
        int ItemID,
        int UnitID,
        short? Factor
    );
}
