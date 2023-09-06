using FinancialHub.Common.Interfaces.Repositories;
using FinancialHub.Core.Domain.Entities;
using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.Interfaces.Repositories
{
    public interface IBalancesRepository : IBaseRepository<BalanceEntity>
    {
        Task<BalanceEntity> ChangeAmountAsync(Guid balanceId,decimal value, TransactionType transactionType, bool removed = false);
        Task<BalanceEntity> ChangeAmountAsync(Guid balanceId,decimal value);
    }
}
