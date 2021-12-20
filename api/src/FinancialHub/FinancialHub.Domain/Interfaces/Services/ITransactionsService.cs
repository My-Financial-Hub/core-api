using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Filters;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface ITransactionsService
    {
        Task<ICollection<TransactionModel>> GetAllByUserAsync(string userId, TransactionFilter filter);

        Task<TransactionModel> CreateAsync(TransactionModel account);

        Task<TransactionModel> UpdateAsync(Guid id, TransactionModel account);

        Task<int> DeleteAsync(Guid id);
    }
}
