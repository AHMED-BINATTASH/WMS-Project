using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Domain.Entities;


namespace WMS.Application.Mappings
{
    public class UnitProfile :Profile
    {
        public UnitProfile()
        {
            CreateMap<Unit,UnitDto>();
            CreateMap<UnitDto,Unit>();
        }
    }
}
