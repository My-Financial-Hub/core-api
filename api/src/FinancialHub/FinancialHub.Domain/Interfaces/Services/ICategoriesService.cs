using FinancialHub.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface ICategoriesService 
    {
        Task<ICollection<CategoryModel>> GetAllByUserAsync(string userId);

        Task<CategoryModel> CreateAsync(CategoryModel category);

        Task<CategoryModel> UpdateAsync(string id, CategoryModel category);

        Task<int> DeleteAsync(string id);
    }
}
