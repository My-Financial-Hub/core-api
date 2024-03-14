using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.DTOS.Transactions;

namespace FinancialHub.Core.Domain.Interfaces.Validators
{
    public interface ITransactionsValidator
    {
        Task<ServiceResult> ExistsAsync(Guid id);
        Task<ServiceResult> ValidateAsync(CreateTransactionDto createTransactionDto);
        Task<ServiceResult> ValidateAsync(UpdateTransactionDto updateTransactionDto);
    }
}
