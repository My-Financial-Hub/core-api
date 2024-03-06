using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;

namespace FinancialHub.Core.Application.Services
{
    public class BalancesService : IBalancesService
    {
        private readonly IBalancesProvider balancesProvider;
        private readonly IBalancesValidator balancesValidator;
        private readonly IErrorMessageProvider errorMessageProvider;
        private readonly IMapper mapper;

        public BalancesService(
            IBalancesProvider balancesProvider,
            IBalancesValidator balancesValidator,
            IErrorMessageProvider errorMessageProvider,
            IMapper mapper
            )
        {
            this.balancesProvider = balancesProvider;
            this.balancesValidator = balancesValidator;
            this.errorMessageProvider = errorMessageProvider;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<BalanceDto>> CreateAsync(CreateBalanceDto balance)
        {
            var validationResult = await this.balancesValidator.ValidateAsync(balance);
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
            var balance = await this.balancesProvider.GetByIdAsync(id);
            if (balance == null)
            {
                return new NotFoundError(
                    this.errorMessageProvider.NotFoundMessage("Balance", id)
                );
            }

            return this.mapper.Map<BalanceDto>(balance);
        }

        public async Task<ServiceResult<ICollection<BalanceDto>>> GetAllByAccountAsync(Guid accountId)
        {
            var validationResult = await this.balancesValidator.AccountExistsAsync(accountId);
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
            var balanceResult = await this.GetByIdAsync(id);
            if (balanceResult.HasError)
            {
                return balanceResult.Error;
            }

            return await balancesProvider.UpdateAmountAsync(id, newAmount);
        }
    }
}
