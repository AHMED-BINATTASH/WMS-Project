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
    public class WarehouseProfile :Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseDto>().ReverseMap();
        }
    }
}
