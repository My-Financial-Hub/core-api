using System;
using System.Threading.Tasks;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface ITransactionBalanceService
    {
        public Task<ServiceResult<TransactionModel>> CreateTransactionAsync(TransactionModel transaction);
    }
}
