using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.Domain.Interfaces.Validators
{
    public interface IBalancesValidator
    {
        Task<ServiceResult> ExistsAsync(Guid id);
        Task<ServiceResult> ValidateAsync(CreateBalanceDto createBalanceDto);
        Task<ServiceResult> ValidateAsync(UpdateBalanceDto updateBalanceDto);
    }
}
