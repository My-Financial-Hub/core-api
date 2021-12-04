using FinancialHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface ITransactionsService
    {
        Task<ICollection<TransactionModel>> GetAllByUserAsync(string userId);

        Task<TransactionModel> CreateAsync(TransactionModel account);

        Task<TransactionModel> UpdateAsync(Guid id, TransactionModel account);

        Task<int> DeleteAsync(Guid id);
    }
}
