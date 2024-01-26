using FinancialHub.Core.Domain.Interfaces.Resources;

namespace FinancialHub.Core.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesProvider provider;
        private readonly IErrorMessageProvider errorMessageProvider;

        public CategoriesService(ICategoriesProvider provider, IErrorMessageProvider errorMessageProvider)
        {
            this.provider = provider;
            this.errorMessageProvider = errorMessageProvider;
        }

        public async Task<ServiceResult<CategoryModel>> CreateAsync(CategoryModel category)
        {
            return await this.provider.CreateAsync(category);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.provider.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<CategoryModel>>> GetAllByUserAsync(string userId)
        {
            var categories = await this.provider.GetAllAsync();

            return categories.ToArray();
        }

        public async Task<ServiceResult<CategoryModel>> UpdateAsync(Guid id, CategoryModel category)
        {
            var existingCategory = await this.provider.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return new NotFoundError(
                    this.errorMessageProvider.NotFoundMessage("Category", id)
                );
            }

            var updatedCategory = await this.provider.UpdateAsync(id, category);
            if (updatedCategory == null)
            {
                return new InvalidDataError(
                    this.errorMessageProvider.UpdateFailedMessage("Category", id)
                );
            }

            return updatedCategory;
        }
    }
}
