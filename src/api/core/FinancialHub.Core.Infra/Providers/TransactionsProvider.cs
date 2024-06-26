﻿using FinancialHub.Core.Domain.Enums;
using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Queries;

namespace FinancialHub.Core.Infra.Providers
{
    internal class TransactionsProvider : ITransactionsProvider
    {
        private readonly IMapper mapper;
        private readonly ITransactionsRepository repository;
        private readonly IBalancesProvider balancesProvider;

        public TransactionsProvider(
            ITransactionsRepository repository, 
            IBalancesProvider balancesProvider,
            IMapper mapper
        )
        {
            this.mapper = mapper;
            this.repository = repository;
            this.balancesProvider = balancesProvider;
        }

        public async Task<TransactionModel> CreateAsync(TransactionModel transaction)
        {
            if (transaction.IsPaid)
            {
                var balance = await this.balancesProvider.GetByIdAsync(transaction.BalanceId);

                decimal newAmount = balance!.Amount;

                if (transaction.Type == TransactionType.Earn)
                {
                    newAmount += transaction.Amount;
                }
                else
                {
                    newAmount -= transaction.Amount;
                }

                await this.balancesProvider.UpdateAmountAsync(transaction.BalanceId, newAmount);
            }

            var entity = mapper.Map<TransactionEntity>(transaction);

            entity = await this.repository.CreateAsync(entity);
            await this.repository.CommitAsync();

            return mapper.Map<TransactionModel>(entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var transaction = await this.repository.GetByIdAsync(id);

            if (transaction?.Status == TransactionStatus.Committed && transaction.IsActive)
            {
                transaction.Balance = null;
                var balanceId = transaction.BalanceId;
                var balance = await this.balancesProvider.GetByIdAsync(balanceId);

                decimal newAmount = balance!.Amount;

                if (transaction.Type == TransactionType.Earn)
                    newAmount -= transaction.Amount;
                else
                    newAmount += transaction.Amount;

                await this.balancesProvider.UpdateAmountAsync(balanceId, newAmount);
            }

            await this.repository.DeleteAsync(id);

            return await this.repository.CommitAsync();
        }

        public async Task<ICollection<TransactionModel>> GetAllAsync(TransactionFilter filter)
        {
            var query = mapper.Map<TransactionQuery>(filter);

            var entities = await this.repository.GetAsync(query.Query());

            return mapper.Map<ICollection<TransactionModel>>(entities);
        }

        public async Task<TransactionModel?> GetByIdAsync(Guid id)
        {
            var entity = await this.repository.GetByIdAsync(id);
            if(entity == null)
                return null;
        
            return mapper.Map<TransactionModel>(entity);
        }

        public async Task<TransactionModel> UpdateAsync(Guid id, TransactionModel transaction)
        {
            var entity = mapper.Map<TransactionEntity>(transaction);
            entity.Id = id;

            var updated = await this.repository.UpdateAsync(entity);
            await this.repository.CommitAsync();
            
            return mapper.Map<TransactionModel>(updated);
        }
    }
}
