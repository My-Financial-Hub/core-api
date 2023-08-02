using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface ICategoriesService 
    {
        Task<ServiceResult<ICollection<CategoryModel>>> GetAllByUserAsync(string userId);

        Task<ServiceResult<CategoryModel>> CreateAsync(CategoryModel category);

        Task<ServiceResult<CategoryModel>> UpdateAsync(Guid id, CategoryModel category);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
