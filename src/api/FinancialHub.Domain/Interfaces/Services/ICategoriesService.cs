using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface ICategoriesService 
    {
        Task<ServiceResult<ICollection<CategoryModel>>> GetAllByUserAsync(string userId);

        Task<ServiceResult<CategoryModel>> CreateAsync(CategoryModel category);

        Task<ServiceResult<CategoryModel>> UpdateAsync(Guid id, CategoryModel category);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
