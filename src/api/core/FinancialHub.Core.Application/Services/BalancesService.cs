namespace FinancialHub.Core.Application.Services
{
    public class BalancesService : IBalancesService
    {
        private readonly IAccountsProvider accountsProvider;
        private readonly IBalancesProvider balancesProvider;

        public BalancesService(
            IBalancesProvider balancesProvider, 
            IAccountsProvider accountsProvider
        )
        {
            this.balancesProvider = balancesProvider;
            this.accountsProvider = accountsProvider;
        }

        private async Task<ServiceResult> ValidateAccountAsync(Guid accountId)
        {
            var account = await this.accountsProvider.GetByIdAsync(accountId);

            if (account == null)
                return new NotFoundError($"Not found Account with id {accountId}");

            return new ServiceResult();
        }

        public async Task<ServiceResult<BalanceModel>> CreateAsync(BalanceModel balance)
        {
            var validationResult = await this.ValidateAccountAsync(balance.AccountId);
            if (validationResult.HasError)
                return validationResult.Error;

            return await this.balancesProvider.CreateAsync(balance);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.balancesProvider.DeleteAsync(id);
        }

        public async Task<ServiceResult<BalanceModel>> GetByIdAsync(Guid id)
        {
            var balance = await this.balancesProvider.GetByIdAsync(id);
            if(balance == null)
                return new NotFoundError($"Not found Balance with id {id}");

            return balance;
        }

        public async Task<ServiceResult<ICollection<BalanceModel>>> GetAllByAccountAsync(Guid accountId)
        {
            var validationResult = await this.ValidateAccountAsync(accountId);
            if (validationResult.HasError)
                return validationResult.Error;

            var accounts = await this.balancesProvider.GetAllByAccountAsync(accountId);

            return accounts.ToArray();
        }

        public async Task<ServiceResult<BalanceModel>> UpdateAsync(Guid id, BalanceModel balance)
        {
            var oldBalance = await this.GetByIdAsync(id);
            if (oldBalance.HasError)
                return oldBalance.Error;

            var validationResult = await this.ValidateAccountAsync(balance.AccountId);
            if (validationResult.HasError)
                return validationResult.Error;

            return await this.balancesProvider.UpdateAsync(id, balance);
        }

        public async Task<ServiceResult<BalanceModel>> UpdateAmountAsync(Guid id, decimal newAmount)
        {
            var balanceResult = await this.GetByIdAsync(id);
            if (balanceResult.HasError)
                return balanceResult.Error;

            return await balancesProvider.UpdateAmountAsync(id, newAmount);
        }
    }
}
