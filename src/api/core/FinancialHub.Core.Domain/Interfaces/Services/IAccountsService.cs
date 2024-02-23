using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface IAccountsService 
    {
        Task<ServiceResult<ICollection<AccountDto>>> GetAllAsync();

        Task<ServiceResult<AccountDto>> GetByIdAsync(Guid id);

        Task<ServiceResult<AccountDto>> CreateAsync(CreateAccountDto accountDto);

        Task<ServiceResult<AccountDto>> UpdateAsync(Guid id, UpdateAccountDto account);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
