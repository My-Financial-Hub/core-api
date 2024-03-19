using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Interfaces.Validators;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesProvider provider;
        private readonly ICategoriesValidator validator;
        private readonly IMapper mapper;
        private readonly ILogger<CategoriesService> logger;

        public CategoriesService(
            ICategoriesProvider provider, ICategoriesValidator validator,
            IMapper mapper, 
            ILogger<CategoriesService> logger
        )
        {
            this.provider = provider;
            this.validator = validator;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto category)
        {
            this.logger.LogInformation("Creating category {name}", category.Name);
            this.logger.LogTrace("Category data : {category}", category);

            var validationResult = await this.validator.ValidateAsync(category);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Category creation Validation result : {validationResult}", validationResult);
                this.logger.LogInformation("Failed creating category {name}", category.Name);
                return validationResult.Error;
            }

            var categoryModel = this.mapper.Map<CategoryModel>(category);

            var createdCategory = await this.provider.CreateAsync(categoryModel);

            var result = this.mapper.Map<CategoryDto>(createdCategory);

            this.logger.LogTrace("Category creation result : {result}", result);
            this.logger.LogInformation("Category {name} Sucessfully created", result.Name);

            return result;
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            this.logger.LogInformation("Removing category {id}", id);
            var amount = await this.provider.DeleteAsync(id);
            this.logger.LogInformation("Category {id} {removed}", id, amount > 0 ? "removed" : "not removed");
            return amount;
        }

        public async Task<ServiceResult<ICollection<CategoryDto>>> GetAllAsync()
        {
            this.logger.LogInformation("Getting all categories");
            var categories = await this.provider.GetAllAsync();

            this.logger.LogInformation("Returning {count} categories", categories.Count > 0 ? $"{categories.Count}" : "no");
            return this.mapper.Map<ICollection<CategoryDto>>(categories).ToArray();
        }

        public async Task<ServiceResult<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryDto category)
        {
            this.logger.LogInformation("Updating category {id}", id);

            var validationResult = await this.validator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Category update validation result : {validationResult}", validationResult);
                this.logger.LogInformation("Failed update category {id}", id); 
                return validationResult.Error;
            }

            validationResult = await this.validator.ValidateAsync(category);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Category update validation result : {validationResult}", validationResult);
                this.logger.LogInformation("Failed update category {id}", id);
                return validationResult.Error;
            }

            var categoryModel = this.mapper.Map<CategoryModel>(category);
            var result = this.mapper.Map<CategoryDto>(categoryModel);

            this.logger.LogTrace("Category update result : {result}", result);
            this.logger.LogInformation("Category {id} Sucessfully Updated", id);

            return result;
        }
    }
}
