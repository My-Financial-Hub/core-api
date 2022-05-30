using FinancialHub.Domain.Entities;
using System.Threading.Tasks;

namespace FinancialHub.Domain.Interfaces.Repositories
{
    public interface IBalancesRepository : IBaseRepository<BalanceEntity>
    {
        Task<BalanceEntity> AddAmountAsync(TransactionEntity transaction);
        Task<BalanceEntity> RemoveAmountAsync(TransactionEntity transaction);
    }
}
