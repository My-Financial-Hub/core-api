using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Interfaces.Resources;

namespace FinancialHub.Core.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesProvider provider;
        private readonly IErrorMessageProvider errorMessageProvider;
        private readonly IMapper mapper;

        public CategoriesService(ICategoriesProvider provider, IErrorMessageProvider errorMessageProvider, IMapper mapper)
        {
            this.provider = provider;
            this.errorMessageProvider = errorMessageProvider;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto category)
        {
            var categoryModel = this.mapper.Map<CategoryModel>(category);

            var createdCategory = await this.provider.CreateAsync(categoryModel);

            return this.mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.provider.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<CategoryDto>>> GetAllByUserAsync(string userId)
        {
            var categories = await this.provider.GetAllAsync();

            return this.mapper.Map<ICollection<CategoryDto>>(categories).ToArray();
        }

        public async Task<ServiceResult<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryDto category)
        {
            var existingCategory = await this.provider.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return new NotFoundError(
                    this.errorMessageProvider.NotFoundMessage("Category", id)
                );
            }

            var categoryModel = this.mapper.Map<CategoryModel>(category);
            var updatedCategory = await this.provider.UpdateAsync(id, categoryModel);
            if (updatedCategory == null)
            {
                return new InvalidDataError(
                    this.errorMessageProvider.UpdateFailedMessage("Category", id)
                );
            }

            return this.mapper.Map<CategoryDto>(updatedCategory);
        }
    }
}
