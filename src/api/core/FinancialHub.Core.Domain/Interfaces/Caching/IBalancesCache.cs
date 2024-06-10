using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Caching
{
    public interface IBalancesCache
    {
        Task AddAsync(BalanceModel balance);
        Task AddAsync(IEnumerable<BalanceModel> balances);
        Task<ICollection<BalanceModel>> GetByAccountAsync(Guid accountId);
        Task<BalanceModel?> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
    }
}
