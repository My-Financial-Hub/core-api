using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface IAccountBalanceService
    {
        Task<ServiceResult<ICollection<BalanceModel>>> GetBalancesByAccountAsync(Guid accountId);
        Task<ServiceResult<AccountModel>> CreateAsync(AccountModel account);
        Task<ServiceResult<int>> DeleteAsync(Guid accountId);
    }
}
