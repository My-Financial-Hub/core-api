using FinancialHub.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface IAccountsService 
    {
        Task<ICollection<AccountModel>> GetAccountsByUserAsync(string userId);
    }
}
