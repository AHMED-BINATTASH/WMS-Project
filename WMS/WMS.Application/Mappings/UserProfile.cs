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
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<User, UserSlimDto>();
            CreateMap<UserDto, UserSlimDto>();

            CreateMap<UserAddDto, User>();
            CreateMap<User, UserAddDto>();

        }
    }
}
