﻿using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Caching
{
    public interface IBalancesCache
    {
        Task AddAsync(BalanceModel balance);
        Task<IEnumerable<BalanceModel>> GetByAccountAsync(Guid accountId);
        Task<BalanceModel?> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
    }
}
