namespace FinancialHub.Core.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesProvider provider;

        public CategoriesService(ICategoriesProvider provider)
        {
            this.provider = provider;
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
                return new NotFoundError($"Not found category with id {id}");

            var updatedCategory = await this.provider.UpdateAsync(id, category);
            if (updatedCategory == null)
                return new InvalidDataError($"Failed to update category {id}");

            return updatedCategory;
        }
    }
}
