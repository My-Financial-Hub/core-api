using FinancialHub.Domain.Models;
using System;

namespace FinancialHub.Domain.Tests.Builders.Models
{
    public class BalanceModelBuilder : BaseModelBuilder<BalanceModel>
    {
        public BalanceModelBuilder() : base()
        {
            var account = new AccountModelBuilder().Generate();

            this.RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            this.RuleFor(x => x.Currency, fake => fake.Finance.Currency().Code);
            this.RuleFor(x => x.Amount, fake => decimal.Round(fake.Random.Decimal(0, 10000), 2));

            this.RuleFor(x => x.Account, fake => account);
            this.RuleFor(x => x.AccountId, fake => account.Id);
        }

        public BalanceModelBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public BalanceModelBuilder WithCurrency(string currency)
        {
            this.RuleFor(c => c.Currency, currency);
            return this;
        }

        public BalanceModelBuilder WithAmount(decimal amount)
        {
            this.RuleFor(c => c.Amount, amount);
            return this;
        }

        public BalanceModelBuilder WithAccount(AccountModel account)
        {
            this.WithAccountId(account.Id);
            this.RuleFor(x => x.Account, fake => account);
            return this;
        }

        public BalanceModelBuilder WithAccountId(Guid? accountId)
        {
            this.RuleFor(x => x.AccountId, fake => accountId);
            this.Ignore(x => x.Account);
            return this;
        }
    }
}
