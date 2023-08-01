using FinancialHub.Core.Domain.Enums;
using FinancialHub.Infra.Data.Contexts;

namespace FinancialHub.Infra.Data.Repositories
{
    public class BalancesRepository : BaseRepository<BalanceEntity>, IBalancesRepository
    {
        public BalancesRepository(FinancialHubContext context) : base(context)
        {
        }

        public async override Task<BalanceEntity> UpdateAsync(BalanceEntity obj)
        {
            obj.UpdateTime = DateTimeOffset.Now;

            var result = this.context.Update(obj);
            this.context.Entry(result.Entity).Property(x => x.Amount).IsModified = false;
            this.context.Entry(result.Entity).Property(x => x.CreationTime).IsModified = false;

            await context.SaveChangesAsync();
            await result.ReloadAsync();

            return result.Entity;
        }

        public async override Task<BalanceEntity> CreateAsync(BalanceEntity obj)
        {
            obj.Amount = 0;
            return await base.CreateAsync(obj);
        }

        public async Task<BalanceEntity> ChangeAmountAsync(Guid balanceId, decimal value, TransactionType transactionType, bool removed = false)
        {
            var balance = await this.GetByIdAsync(balanceId);

            if (transactionType == TransactionType.Earn)
            {
                balance.Amount = !removed ? balance.Amount + value: balance.Amount - value;
            }
            else
            {
                balance.Amount = removed ? balance.Amount + value : balance.Amount - value;
            }

            balance.UpdateTime = DateTimeOffset.Now;
            context.Update(balance);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            return balance;
        }

        public async Task<BalanceEntity> ChangeAmountAsync(Guid balanceId, decimal value)
        {
            var balance = await this.GetByIdAsync(balanceId);
            context.ChangeTracker.Clear();
            balance.Amount = value;
            return await this.UpdateAsync(balance);
        }
    }
}
