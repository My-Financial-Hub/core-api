using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface ICategoriesService 
    {
        Task<ServiceResult<ICollection<CategoryDto>>> GetAllByUserAsync(string userId);

        Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto category);

        Task<ServiceResult<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryDto category);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
