using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Caching
{
    public interface IAccountsCache
    {
        Task AddAsync(AccountModel account);
        Task<AccountModel?> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
    }
}
