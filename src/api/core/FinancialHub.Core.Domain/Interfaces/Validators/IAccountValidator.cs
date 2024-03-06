using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.Domain.Interfaces.Validators
{
    public interface IAccountValidator
    {
        Task<ServiceResult> ExistsAsync(Guid id);
        Task<ServiceResult> ValidateAsync(CreateAccountDto createAccountDto);
        Task<ServiceResult> ValidateAsync(UpdateAccountDto updateAccountDto);
    }
}
