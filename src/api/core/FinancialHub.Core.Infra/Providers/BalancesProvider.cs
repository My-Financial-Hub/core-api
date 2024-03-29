﻿using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Infra.Providers
{
    public class BalancesProvider : IBalancesProvider
    {
        private readonly IMapper mapper;
        private readonly IBalancesRepository repository;

        public BalancesProvider(IMapper mapper,IBalancesRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<BalanceModel> CreateAsync(BalanceModel balance)
        {
            var entity = this.mapper.Map<BalanceEntity>(balance);
            entity.Amount = 0;

            entity = await this.repository.CreateAsync(entity);
            await this.repository.CommitAsync();

            return mapper.Map<BalanceModel>(entity);
        }

        public async Task<BalanceModel> DecreaseAmountAsync(Guid balanceId, decimal amount, TransactionType type)
        {
            var balance = await this.repository.GetByIdAsync(balanceId);

            decimal newAmount = balance!.Amount;

            if (type == TransactionType.Earn)
                newAmount -= amount;
            else
                newAmount += amount;

            var newBalance = await repository.ChangeAmountAsync(balanceId, newAmount);
            await this.repository.CommitAsync();

            return mapper.Map<BalanceModel>(newBalance);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            await this.repository.DeleteAsync(id);
            return await this.repository.CommitAsync();
        }

        public async Task<ICollection<BalanceModel>> GetAllByAccountAsync(Guid accountId)
        {
            var entities = await this.repository.GetAsync(x => x.AccountId == accountId);

            return this.mapper.Map<ICollection<BalanceModel>>(entities);
        }

        public async Task<BalanceModel?> GetByIdAsync(Guid id)
        {
            var balance = await this.repository.GetByIdAsync(id);

            if (balance == null)
                return null;

            return mapper.Map<BalanceModel>(balance);
        }

        public async Task<BalanceModel> IncreaseAmountAsync(Guid balanceId, decimal amount, TransactionType type)
        {
            var balance = await this.repository.GetByIdAsync(balanceId);

            decimal newAmount = balance!.Amount;

            if (type == TransactionType.Earn)
                newAmount += amount;
            else
                newAmount -= amount;

            var newBalance = await repository.ChangeAmountAsync(balanceId, newAmount);
            await this.repository.CommitAsync();

            return mapper.Map<BalanceModel>(newBalance);
        }

        public async Task<BalanceModel> UpdateAmountAsync(Guid id, decimal newAmount)
        {
            var newBalance = await repository.ChangeAmountAsync(id, newAmount);
            await this.repository.CommitAsync();

            return mapper.Map<BalanceModel>(newBalance);
        }

        public async Task<BalanceModel> UpdateAsync(Guid id, BalanceModel balance)
        {
            var balanceEntity = mapper.Map<BalanceEntity>(balance);
            balanceEntity.Id = id;

            var updatedBalance = await this.repository.UpdateAsync(balanceEntity);
            await this.repository.CommitAsync();

            return mapper.Map<BalanceModel>(updatedBalance);
        }
    }
}
