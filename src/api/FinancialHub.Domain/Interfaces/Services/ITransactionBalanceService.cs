using System;
using System.Threading.Tasks;
using FinancialHub.Common.Results;
using FinancialHub.Domain.Models;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface ITransactionBalanceService
    {
        Task<ServiceResult<TransactionModel>> CreateTransactionAsync(TransactionModel transaction);
        Task<ServiceResult<TransactionModel>> UpdateTransactionAsync(Guid id,TransactionModel transaction);
        Task UpdateAmountAsync(TransactionModel oldTransaction, TransactionModel newTransaction);
        Task<ServiceResult<bool>> DeleteTransactionAsync(Guid id);
    }
}
