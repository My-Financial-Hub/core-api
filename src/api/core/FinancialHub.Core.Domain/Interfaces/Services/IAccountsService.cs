using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface IAccountsService 
    {
        Task<ServiceResult<ICollection<AccountDto>>> GetAllByUserAsync(string userId);

        Task<ServiceResult<AccountDto>> GetByIdAsync(Guid id);

        Task<ServiceResult<AccountDto>> CreateAsync(CreateAccountDto accountDto);

        Task<ServiceResult<AccountDto>> UpdateAsync(Guid id, UpdateAccountDto account);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
