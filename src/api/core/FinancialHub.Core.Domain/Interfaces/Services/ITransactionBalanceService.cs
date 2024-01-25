using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface ITransactionBalanceService
    {
        Task<ServiceResult<TransactionModel>> CreateTransactionAsync(TransactionModel transaction);
        Task<ServiceResult<TransactionModel>> UpdateTransactionAsync(Guid id,TransactionModel transaction);
        Task UpdateAmountAsync(TransactionModel oldTransaction, TransactionModel newTransaction);
    }
}
