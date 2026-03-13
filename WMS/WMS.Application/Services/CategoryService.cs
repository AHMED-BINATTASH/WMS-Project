using AutoMapper;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddNew(Category Entity)
        {
            return await _categoryRepository.Add(Entity); 
        }
        public async Task<bool> Delete(int id)
        {
            return await _categoryRepository.Delete(id);
        }

        public async Task<IEnumerable<CategoryDto>?> GetAll()
        {
            IEnumerable<Category> Cateories = await _categoryRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<CategoryDto>>(Cateories);
        }

        public async Task<CategoryDto?> GetByID(int id)
        {
            Category Category = await _categoryRepository.GetByIdAsync(id);
            return Category != null ? _mapper.Map<CategoryDto>(Category) : null;
        }


        async public Task<bool> Update(Category Entity)
        {
            return await _categoryRepository.Update(Entity);
        }
        public async Task<bool> IsExistByName(string categoryName)
        {
           return await _categoryRepository.IsExistByName(categoryName);
        }
    }
}
