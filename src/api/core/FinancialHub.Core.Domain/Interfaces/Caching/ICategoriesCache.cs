using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Caching
{
    public interface ICategoriesCache
    {
        Task AddAsync(CategoryModel category);
        Task<CategoryModel?> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
    }
}
