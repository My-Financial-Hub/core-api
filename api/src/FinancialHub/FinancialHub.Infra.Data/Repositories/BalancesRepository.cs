using System;
using System.Threading.Tasks;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Enums;
using FinancialHub.Domain.Interfaces.Repositories;
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

        protected async Task<BalanceEntity> UpdateAmountAsync(BalanceEntity balance)
        {
            balance.UpdateTime = DateTimeOffset.Now;

            context.Update(balance);
            await context.SaveChangesAsync();

            return balance;
        }

        public async Task<BalanceEntity> AddAmountAsync(TransactionEntity transaction)
        {
            var balance = await this.GetByIdAsync(transaction.BalanceId);

            if (transaction.Type == TransactionType.Earn)
            {
                balance.Amount += transaction.Amount;
            }
            else
            {
                balance.Amount -= transaction.Amount;
            }

            return await this.UpdateAmountAsync(balance);
        }

        public async Task<BalanceEntity> RemoveAmountAsync(TransactionEntity transaction)
        {
            var balance = await this.GetByIdAsync(transaction.BalanceId);

            if (transaction.Type == TransactionType.Earn)
            {
                balance.Amount -= transaction.Amount;
            }
            else
            {
                balance.Amount += transaction.Amount;
            }

            return await this.UpdateAmountAsync(balance);
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

            return this.context.Entry(balance).Entity;
        }
    }
}
