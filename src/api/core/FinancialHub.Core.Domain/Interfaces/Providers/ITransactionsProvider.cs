using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Providers
{
    public interface ITransactionsProvider
    {
        Task<ICollection<TransactionModel>> GetAllAsync(TransactionFilter filter);

        Task<TransactionModel> GetByIdAsync(Guid id);

        Task<TransactionModel> CreateAsync(TransactionModel transaction);

        Task<TransactionModel> UpdateAsync(Guid id, TransactionModel transaction);

        Task<int> DeleteAsync(Guid id);
    }
}
