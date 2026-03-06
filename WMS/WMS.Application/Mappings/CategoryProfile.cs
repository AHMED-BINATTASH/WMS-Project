using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
              CreateMap<Category, CategoryDto>();
              CreateMap<CategoryDto, Category>();
        }
    }
}
