using FinancialHub.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface IAccountsService 
    {
        Task<ICollection<AccountModel>> GetAllByUserAsync(string userId);

        Task<AccountModel> CreateAsync(AccountModel account);

        Task<AccountModel> UpdateAsync(string id,AccountModel account);

        Task<int> DeleteAsync(string id);
    }
}
