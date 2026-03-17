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

            CreateMap<User, UserSlimDto>();

            CreateMap<UserAddDto, User>()
                .ForMember(dest => dest.UserID, opt => opt.Ignore()); 

            CreateMap<UserSlimDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<UserDto, User>();
            CreateMap<User, UserAddDto>();
            CreateMap<UserDto, UserSlimDto>();
            CreateMap<UserSlimDto, UserDto>();

            CreateMap<UserAddDto, UserSlimDto>();
        }
    }
}
