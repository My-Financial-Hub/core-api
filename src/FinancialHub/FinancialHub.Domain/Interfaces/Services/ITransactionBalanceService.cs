using System;
using System.Threading.Tasks;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface ITransactionBalanceService
    {
        Task<ServiceResult<TransactionModel>> CreateTransactionAsync(TransactionModel transaction);
        Task<ServiceResult<TransactionModel>> UpdateTransactionAsync(Guid id,TransactionModel transaction);
        Task UpdateAmountAsync(TransactionModel oldTransaction, TransactionModel newTransaction);
    }
}
