using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;

namespace FinancialHub.Core.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesProvider provider;
        private readonly ICategoriesValidator validator;
        private readonly IMapper mapper;

        public CategoriesService(
            ICategoriesProvider provider, 
            ICategoriesValidator validator,
            IMapper mapper
        )
        {
            this.provider = provider;
            this.validator = validator;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto category)
        {
            var validation = await this.validator.ValidateAsync(category);
            if (validation.HasError)
            {
                return validation.Error;
            }

            var categoryModel = this.mapper.Map<CategoryModel>(category);

            var createdCategory = await this.provider.CreateAsync(categoryModel);

            return this.mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.provider.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<CategoryDto>>> GetAllAsync()
        {
            var categories = await this.provider.GetAllAsync();

            return this.mapper.Map<ICollection<CategoryDto>>(categories).ToArray();
        }

        public async Task<ServiceResult<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryDto category)
        {
            var existingCategory = await this.validator.ExistsAsync(id);
            if (existingCategory.HasError)
            {
                return existingCategory.Error;
            }

            var validation = await this.validator.ValidateAsync(category);
            if (validation.HasError)
            {
                return validation.Error;
            }

            var categoryModel = this.mapper.Map<CategoryModel>(category);
            var updatedCategory = await this.provider.UpdateAsync(id, categoryModel);

            return this.mapper.Map<CategoryDto>(updatedCategory);
        }
    }
}
