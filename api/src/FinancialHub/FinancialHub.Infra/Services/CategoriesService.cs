using System;
using AutoMapper;
using System.Threading.Tasks;
using FinancialHub.Domain.Models;
using System.Collections.Generic;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Interfaces.Repositories;

namespace FinancialHub.Infra.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IMapper mapper;
        private readonly ICategoriesRepository repository;

        public CategoriesService(IMapper mapper, ICategoriesRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<CategoryModel> CreateAsync(CategoryModel category)
        {
            var entity = mapper.Map<CategoryEntity>(category);
            entity.CreationTime = DateTimeOffset.Now;
            entity.UpdateTime = DateTimeOffset.Now;

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<CategoryModel>(entity);
        }

        public async Task<int> DeleteAsync(string id)
        {
            return await this.repository.DeleteAsync(id);
        }

        public async Task<ICollection<CategoryModel>> GetAllByUserAsync(string userId)
        {
            var entities = await this.repository.GetAllAsync();
            return mapper.Map<ICollection<CategoryModel>>(entities);
        }

        public async Task<CategoryModel> UpdateAsync(string id, CategoryModel category)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new NullReferenceException($"Not found category with id {id}");
            }
            entity.Id = new Guid(id);

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<CategoryModel>(entity);
        }
    }
}
