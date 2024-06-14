using AutoMapper;
using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.Balances.Models;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Domain.Balances.Services
{
    public class BalancesService
    {
        private readonly IMapper mapper;
        private readonly ILogger<BalancesService> logger;

        public BalancesService(IMapper mapper, ILogger<BalancesService> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ServiceResult<Balance>> CreateAsync(CreateBalanceDto createBalance)
        {
            throw new NotImplementedException();
        }
    }
}
