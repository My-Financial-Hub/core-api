using System;
using System.Threading.Tasks;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using System.Collections.Generic;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface IAccountsService 
    {
        Task<ServiceResult<IEnumerable<AccountModel>>> GetAllByUserAsync(string userId);

        Task<ServiceResult<AccountModel>> GetByIdAsync(Guid id);

        Task<ServiceResult<AccountModel>> CreateAsync(AccountModel account);

        Task<ServiceResult<AccountModel>> UpdateAsync(Guid id,AccountModel account);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
