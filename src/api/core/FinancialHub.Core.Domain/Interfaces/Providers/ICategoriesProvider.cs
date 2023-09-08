using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Providers
{
    public interface ICategoriesProvider
    {
        Task<ICollection<CategoryModel>> GetAllAsync();

        Task<CategoryModel?> GetByIdAsync(Guid id);

        Task<CategoryModel> CreateAsync(CategoryModel category);

        Task<CategoryModel?> UpdateAsync(Guid id, CategoryModel category);

        Task<int> DeleteAsync(Guid id);
    }
}
