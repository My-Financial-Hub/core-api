using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.Models;
using FinancialHub.Core.Domain.Filters;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface ITransactionsService
    {
        Task<ServiceResult<ICollection<TransactionModel>>> GetAllByUserAsync(string userId, TransactionFilter filter);

        Task<ServiceResult<TransactionModel>> GetByIdAsync(Guid id);

        Task<ServiceResult<TransactionModel>> CreateAsync(TransactionModel transaction);

        Task<ServiceResult<TransactionModel>> UpdateAsync(Guid id, TransactionModel transaction);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
