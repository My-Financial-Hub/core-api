using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace FinancialHub.Domain.Interfaces.Repositories
{
    public interface IBalancesRepository : IBaseRepository<BalanceEntity>
    {
        Task<BalanceEntity> ChangeAmountAsync(Guid balanceId,decimal value, TransactionType transactionType, bool removed = false);
        Task<BalanceEntity> AddAmountAsync(TransactionEntity transaction);
        Task<BalanceEntity> RemoveAmountAsync(TransactionEntity transaction);
    }
}
