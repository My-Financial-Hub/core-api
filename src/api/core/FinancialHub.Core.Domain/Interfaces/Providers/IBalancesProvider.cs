using FinancialHub.Core.Domain.Enums;
using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Providers
{
    public interface IBalancesProvider
    {
        Task<BalanceModel?> GetByIdAsync(Guid id);

        Task<ICollection<BalanceModel>> GetAllByAccountAsync(Guid accountId);

        Task<BalanceModel> CreateAsync(BalanceModel balance);

        Task<BalanceModel> UpdateAsync(Guid id, BalanceModel balance);

        Task<BalanceModel> UpdateAmountAsync(Guid id, decimal newAmount);

        Task<BalanceModel> IncreaseAmountAsync(Guid balanceId, decimal amount, TransactionType type);

        Task<BalanceModel> DecreaseAmountAsync(Guid balanceId, decimal amount, TransactionType type);

        Task<int> DeleteAsync(Guid id);
    }
}
