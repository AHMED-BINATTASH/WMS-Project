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
    public class CategoryService : IService<CategoryDto, Category>
    {
        private readonly IRepository<Category> _repository;
        private readonly IMapper _mapper;
        public CategoryService(IRepository<Category> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<bool> AddNew(Category Entity)
        {
            if (Entity == null)
                return false;


            var IsAdded = await _repository.Add(Entity);
            return IsAdded;


        }


        public async Task<bool> Delete(int id)
        {
            var IsDeleted = await _repository.Delete(id);

            return IsDeleted;
        }

        public async Task<IEnumerable<CategoryDto>?> GetAll()
        {
            var Cateories = await _repository.GetAllAsync();

            return Cateories != null ? _mapper.Map<IEnumerable<CategoryDto>>(Cateories) : null;
        }

        public async Task<CategoryDto?> GetByID(int id)
        {
            var Category = await _repository.GetByIdAsync(id);
            return Category != null ? _mapper.Map<CategoryDto>(Category) : null;
        }


        async public Task<bool> Update(Category Entity)
        {
            if (Entity == null)
                return false;

            var existingCategory = await _repository.GetByIdAsync(Entity.CategoryID);

            if (existingCategory == null)
                return false;

            existingCategory.CategoryName = Entity.CategoryName;
            existingCategory.Description = Entity.Description;
            existingCategory.ParentCategoryId = Entity.ParentCategoryId;

            return await _repository.Update(existingCategory);
        }
    }
}
