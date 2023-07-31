using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialHub.Common.Results;
using FinancialHub.Domain.Models;

namespace FinancialHub.Common.Interfaces.Services
{
    public interface IAccountsService 
    {
        Task<ServiceResult<ICollection<AccountModel>>> GetAllByUserAsync(string userId);

        Task<ServiceResult<AccountModel>> GetByIdAsync(Guid id);

        Task<ServiceResult<AccountModel>> CreateAsync(AccountModel account);

        Task<ServiceResult<AccountModel>> UpdateAsync(Guid id,AccountModel account);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
