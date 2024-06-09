using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Caching
{
    [Obsolete("No use for it. Filters are too complex to use cache")]
    public interface ITransactionsCache
    {
        Task AddAsync(TransactionModel transaction);
        Task<ICollection<TransactionModel>> GetAsync(params Guid[] balances);
        Task<ICollection<TransactionModel>> GetAsync(TransactionFilter filter);
        Task RemoveAsync(Guid id);
    }
}
