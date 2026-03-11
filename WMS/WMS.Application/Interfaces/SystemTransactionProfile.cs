using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces
{
    public class SystemTransactionProfile :Profile
    {
        public SystemTransactionProfile()
        {
            CreateMap<SystemTransaction, SystemTransactionDto>().ReverseMap();

        }
    }
}
