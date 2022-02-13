using FinancialHub.Domain.Models;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Mappers;

namespace FinancialHub.Services.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IMapperWrapper mapper;
        private readonly ICategoriesRepository repository;

        public CategoriesService(IMapperWrapper mapper, ICategoriesRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<CategoryModel> CreateAsync(CategoryModel category)
        {
            var entity = mapper.Map<CategoryEntity>(category);

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<CategoryModel>(entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await this.repository.DeleteAsync(id);
        }

        public async Task<ICollection<CategoryModel>> GetAllByUserAsync(string userId)
        {
            var entities = await this.repository.GetAllAsync();
            return mapper.Map<ICollection<CategoryModel>>(entities);
        }

        public async Task<CategoryModel> UpdateAsync(Guid id, CategoryModel category)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new NullReferenceException($"Not found category with id {id}");
            }
            entity.Id = id;

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<CategoryModel>(entity);
        }
    }
}
