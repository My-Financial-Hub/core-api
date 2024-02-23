using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface IBalancesService
    {
        Task<ServiceResult<BalanceDto>> GetByIdAsync(Guid id);

        Task<ServiceResult<ICollection<BalanceDto>>> GetAllByAccountAsync(Guid accountId);

        Task<ServiceResult<BalanceDto>> CreateAsync(CreateBalanceDto balance);

        Task<ServiceResult<BalanceDto>> UpdateAsync(Guid id, UpdateBalanceDto balance);
        [Obsolete("This logic will be moved to BalanceModel")]
        Task<ServiceResult<BalanceModel>> UpdateAmountAsync(Guid id, decimal newAmount);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
