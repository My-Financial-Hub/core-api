using Bogus;
using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances
{
    public class CreateBalanceDtoBuilder : Faker<CreateBalanceDto>
    {
        public CreateBalanceDtoBuilder() : base()
        {
            this.RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            this.RuleFor(x => x.Currency, fake => fake.Finance.Currency().Code);
            this.RuleFor(x => x.AccountId, fake => fake.Random.Uuid());
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public CreateBalanceDtoBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public CreateBalanceDtoBuilder WithCurrency(string currency)
        {
            this.RuleFor(c => c.Currency, currency);
            return this;
        }

        public CreateBalanceDtoBuilder WithAccountId(Guid? accountId)
        {
            this.RuleFor(x => x.AccountId, fake => accountId);
            return this;
        }
    }
}
