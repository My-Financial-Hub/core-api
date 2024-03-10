using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Interfaces.Validators;

namespace FinancialHub.Core.Application.Services
{
    public class BalancesService : IBalancesService
    {
        private readonly IBalancesProvider balancesProvider;
        private readonly IBalancesValidator balancesValidator;
        private readonly IAccountsValidator accountsValidator;
        private readonly IMapper mapper;

        public BalancesService(
            IBalancesProvider balancesProvider,
            IBalancesValidator balancesValidator, IAccountsValidator accountsValidator,
            IMapper mapper
        )
        {
            this.balancesProvider = balancesProvider;
            this.balancesValidator = balancesValidator;
            this.accountsValidator = accountsValidator;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<BalanceDto>> CreateAsync(CreateBalanceDto balance)
        {
            var validationResult = await this.balancesValidator.ValidateAsync(balance);
            if (validationResult.HasError)
                return validationResult.Error;

            validationResult = await this.accountsValidator.ExistsAsync(balance.AccountId);
            if (validationResult.HasError)
                return validationResult.Error;

            var balanceModel = this.mapper.Map<BalanceModel>(balance);

            var createdBalance = await this.balancesProvider.CreateAsync(balanceModel);

            return this.mapper.Map<BalanceDto>(createdBalance);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.balancesProvider.DeleteAsync(id);
        }

        public async Task<ServiceResult<BalanceDto>> GetByIdAsync(Guid id)
        {
            var validationResult = await this.balancesValidator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                return validationResult.Error;
            }

            var balance = await this.balancesProvider.GetByIdAsync(id);

            return this.mapper.Map<BalanceDto>(balance);
        }

        public async Task<ServiceResult<ICollection<BalanceDto>>> GetAllByAccountAsync(Guid accountId)
        {
            var validationResult = await this.accountsValidator.ExistsAsync(accountId);
            if (validationResult.HasError)
            {
                return validationResult.Error;
            }

            var balances = await this.balancesProvider.GetAllByAccountAsync(accountId);

            return this.mapper.Map<ICollection<BalanceDto>>(balances).ToArray();
        }

        public async Task<ServiceResult<BalanceDto>> UpdateAsync(Guid id, UpdateBalanceDto balance)
        {
            var oldBalance = await this.balancesValidator.ExistsAsync(id);
            if (oldBalance.HasError)
            {
                return oldBalance.Error;
            }

            var validationResult = await this.balancesValidator.ValidateAsync(balance);
            if (validationResult.HasError)
            {
                return validationResult.Error;
            }

            var balanceModel = this.mapper.Map<BalanceModel>(balance);

            var updatedBalance = await this.balancesProvider.UpdateAsync(id, balanceModel);

            return this.mapper.Map<BalanceDto>(updatedBalance);
        }

        public async Task<ServiceResult<BalanceModel>> UpdateAmountAsync(Guid id, decimal newAmount)
        {
            var oldBalance = await this.balancesValidator.ExistsAsync(id);
            if (oldBalance.HasError)
            {
                return oldBalance.Error;
            }

            return await balancesProvider.UpdateAmountAsync(id, newAmount);
        }
    }
}
