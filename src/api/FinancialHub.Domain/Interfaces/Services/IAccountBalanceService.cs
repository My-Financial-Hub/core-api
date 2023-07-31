using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialHub.Common.Results;
using FinancialHub.Domain.Models;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface IAccountBalanceService
    {
        Task<ServiceResult<ICollection<BalanceModel>>> GetBalancesByAccountAsync(Guid accountId);
        Task<ServiceResult<AccountModel>> CreateAsync(AccountModel account);
        Task<ServiceResult<int>> DeleteAsync(Guid accountId);
    }
}
