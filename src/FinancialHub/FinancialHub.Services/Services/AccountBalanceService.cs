using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;

namespace FinancialHub.Services.Services
{
    public class AccountBalanceService : IAccountBalanceService
    {
        private readonly IBalancesService balancesService;
        private readonly IAccountsService accountsService;

        public AccountBalanceService(IBalancesService balancesService,IAccountsService accountsService)
        {
            this.balancesService = balancesService;
            this.accountsService = accountsService;
        }

        public async Task<ServiceResult<AccountModel>> CreateAsync(AccountModel account)
        {
            var createdAccount = await this.accountsService.CreateAsync(account);

            if(createdAccount.HasError)
            {
                return createdAccount.Error;
            }

            var balance = new BalanceModel()
            {
                Name = $"{createdAccount.Data.Name} Default Balance",
                AccountId = createdAccount.Data.Id.GetValueOrDefault(),
                IsActive = createdAccount.Data.IsActive
            };
            var createdBalance = await this.balancesService.CreateAsync(balance);

            if (createdBalance.HasError)
            {
                return createdBalance.Error;
            }

            return createdAccount;
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid accountId)
        {
            int removedLines = 0;

            var balances = await this.balancesService.GetAllByAccountAsync(accountId);

            foreach (var balance in balances.Data)
            {
                var balanceResult = await this.balancesService.DeleteAsync(balance.Id.GetValueOrDefault());
                if (balanceResult.HasError)
                {
                    return balanceResult.Error;
                }

                removedLines += balanceResult.Data;
            }

            var accountResult = await this.accountsService.DeleteAsync(accountId);
            if (accountResult.HasError)
            {
                return accountResult.Error;
            }

            removedLines += accountResult.Data;

            return removedLines;
        }

        public async Task<ServiceResult<ICollection<BalanceModel>>> GetBalancesByAccountAsync(Guid accountId)
        {
            return await this.balancesService.GetAllByAccountAsync(accountId);
        }
    }
}
