using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Providers
{
    public interface IAccountsProvider
    {
        Task<ICollection<AccountModel>> GetAllAsync();

        Task<AccountModel?> GetByIdAsync(Guid id);

        Task<AccountModel> CreateAsync(AccountModel account);

        Task<AccountModel> UpdateAsync(Guid id, AccountModel account);

        Task<int> DeleteAsync(Guid id);
    }
}
