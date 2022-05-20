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

            if(createdAccount == null)
            {
                return new ServiceResult<AccountModel>();
            }

            var balance = new BalanceModel()
            {
                Name = "Default Balance",
                AccountId = createdAccount.Data.Id.GetValueOrDefault()
            };
            var createdBalance = await this.balancesService.CreateAsync(balance);

            if (createdBalance == null)
            {
                return new ServiceResult<AccountModel>();
            }

            return createdAccount;
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid accountId)
        {
            int removedLines = 0;
            
            var accountResult = await this.accountsService.DeleteAsync(accountId);
            removedLines += accountResult.Data;

            var balances = await this.balancesService.GetAllByAccountAsync(accountId);

            foreach (var balance in balances.Data)
            {
                var balanceResult = await this.balancesService.DeleteAsync(balance.Id.GetValueOrDefault());
                removedLines += balanceResult.Data;
            }

            return removedLines;
        }
    }
}
