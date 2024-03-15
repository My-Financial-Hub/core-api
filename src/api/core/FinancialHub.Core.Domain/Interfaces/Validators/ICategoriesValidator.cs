using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.Domain.Interfaces.Validators
{
    public interface ICategoriesValidator
    {
        Task<ServiceResult> ExistsAsync(Guid id);
        Task<ServiceResult> ValidateAsync(CreateCategoryDto createCategoryDto);
        Task<ServiceResult> ValidateAsync(UpdateCategoryDto updateCategoryDto);
    }
}
