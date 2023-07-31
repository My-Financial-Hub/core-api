using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialHub.Common.Results;
using FinancialHub.Domain.Models;

namespace FinancialHub.Domain.Interfaces.Services
{
    public interface IBalancesService
    {
        Task<ServiceResult<BalanceModel>> GetByIdAsync(Guid id);

        Task<ServiceResult<ICollection<BalanceModel>>> GetAllByAccountAsync(Guid accountId);

        Task<ServiceResult<BalanceModel>> CreateAsync(BalanceModel balance);

        Task<ServiceResult<BalanceModel>> UpdateAsync(Guid id, BalanceModel balance);

        Task<ServiceResult<BalanceModel>> UpdateAmountAsync(Guid id, decimal newAmount);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
