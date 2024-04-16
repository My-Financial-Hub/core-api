using FinancialHub.Core.Domain.Enums;
using FinancialHub.Core.Domain.Interfaces.Caching;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Providers
{
    public class BalancesProvider : IBalancesProvider
    {
        private readonly IMapper mapper;    
        private readonly IBalancesRepository repository;
        private readonly IBalancesCache cache;
        private readonly ILogger<BalancesProvider> logger;

        public BalancesProvider(
            IBalancesRepository repository,
            IBalancesCache cache,
            IMapper mapper,
            ILogger<BalancesProvider> logger
        )
        {
            this.repository = repository;
            this.cache = cache;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<BalanceModel> CreateAsync(BalanceModel balance)
        {
            var entity = this.mapper.Map<BalanceEntity>(balance);
            entity.Amount = 0;

            entity = await this.repository.CreateAsync(entity);
            await this.repository.CommitAsync();

            var balanceModel = mapper.Map<BalanceModel>(entity);
            await this.cache.AddAsync(balanceModel);

            return balanceModel;
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

            var result = mapper.Map<BalanceModel>(newBalance);
            await this.cache.AddAsync(result);

            return result;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            await this.repository.DeleteAsync(id);
            await this.cache.RemoveAsync(id);
            return await this.repository.CommitAsync();
        }

        public async Task<ICollection<BalanceModel>> GetAllByAccountAsync(Guid accountId)
        {
            var balances = await this.cache.GetByAccountAsync(accountId);
            if(balances == null)
                return Array.Empty<BalanceModel>();
           
            var entities = await this.repository.GetAsync(x => x.AccountId == accountId);

            var result = this.mapper.Map<ICollection<BalanceModel>>(entities);
            await this.cache.AddAsync(result);
            
            return result;
        }

        public async Task<BalanceModel?> GetByIdAsync(Guid id)
        {
            var balanceModel = await this.cache.GetAsync(id);
            if(balanceModel != null)
                return balanceModel;

            var balanceEntity = await this.repository.GetByIdAsync(id);
            if (balanceEntity == null)
                return null;

            var result = mapper.Map<BalanceModel>(balanceEntity);
            await this.cache.AddAsync(result);

            return result;
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

            var result = mapper.Map<BalanceModel>(newBalance);
            await this.cache.AddAsync(result);
            
            return result;
        }

        public async Task<BalanceModel> UpdateAmountAsync(Guid id, decimal newAmount)
        {
            var newBalance = await repository.ChangeAmountAsync(id, newAmount);
            await this.repository.CommitAsync();

            var result = mapper.Map<BalanceModel>(newBalance);
            await this.cache.AddAsync(result);

            return result;
        }

        public async Task<BalanceModel> UpdateAsync(Guid id, BalanceModel balance)
        {
            var balanceEntity = mapper.Map<BalanceEntity>(balance);
            balanceEntity.Id = id;

            var updatedBalance = await this.repository.UpdateAsync(balanceEntity);
            await this.repository.CommitAsync();

            var result = mapper.Map<BalanceModel>(updatedBalance);
            await this.cache.AddAsync(result);

            return result;
        }
    }
}
