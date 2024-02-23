using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.DTOS.Transactions;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface ITransactionsService
    {
        Task<ServiceResult<ICollection<TransactionDto>>> GetAllAsync(TransactionFilter filter);

        Task<ServiceResult<TransactionDto>> GetByIdAsync(Guid id);

        Task<ServiceResult<TransactionDto>> CreateAsync(CreateTransactionDto transaction);

        Task<ServiceResult<TransactionDto>> UpdateAsync(Guid id, UpdateTransactionDto transaction);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
