using Bogus;
using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Tests.Builders.Models;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances
{
    public class UpdateBalanceDtoBuilder : Faker<UpdateBalanceDto>
    {
        public UpdateBalanceDtoBuilder() : base()
        {
            this.RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            this.RuleFor(x => x.Currency, fake => fake.Finance.Currency().Code);
            this.RuleFor(x => x.AccountId, fake => fake.Random.Uuid());
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public UpdateBalanceDtoBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public UpdateBalanceDtoBuilder WithCurrency(string currency)
        {
            this.RuleFor(c => c.Currency, currency);
            return this;
        }

        public UpdateBalanceDtoBuilder WithAccountId(Guid? accountId)
        {
            this.RuleFor(x => x.AccountId, fake => accountId);
            return this;
        }
    }
}
