using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Caching
{
    public interface IAccountCache
    {
        Task AddAsync(Guid id, AccountModel account);
        Task<AccountModel?> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
    }
}
