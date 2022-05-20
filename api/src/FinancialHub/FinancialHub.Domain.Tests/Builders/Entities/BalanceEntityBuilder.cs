using FinancialHub.Domain.Entities;
using System;

namespace FinancialHub.Domain.Tests.Builders.Entities
{
    public class BalanceEntityBuilder : BaseEntityBuilder<BalanceEntity>
    {
        public BalanceEntityBuilder() : base()
        {
            var account = new AccountEntityBuilder().Generate();

            this.RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            this.RuleFor(x => x.Currency, fake => fake.Finance.Currency().Code);
            this.RuleFor(x => x.Amount, fake => decimal.Round(fake.Random.Decimal(0, 10000), 2));

            this.RuleFor(x => x.Account, fake => account);
            this.RuleFor(x => x.AccountId, fake => account.Id);
        }

        public BalanceEntityBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public BalanceEntityBuilder WithCurrency(string currency)
        {
            this.RuleFor(c => c.Currency, currency);
            return this;
        }

        public BalanceEntityBuilder WithAmount(decimal amount)
        {
            this.RuleFor(c => c.Amount, amount);
            return this;
        }

        public BalanceEntityBuilder WithAccount(AccountEntity account)
        {
            this.WithAccountId(account.Id);
            this.RuleFor(x => x.Account, fake => account);
            return this;
        }

        public BalanceEntityBuilder WithAccountId(Guid? accountId)
        {
            this.RuleFor(x => x.AccountId, fake => accountId);
            this.RuleFor(x => x.Account, fake => default);
            return this;
        }
    }
}
