using Bogus;
using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances
{
    public class BalanceDtoBuilder : Faker<BalanceDto>
    {
        private readonly BalanceAccountDtoBuilder balanceAccountDtoBuilder;
        public BalanceDtoBuilder() : base()
        {
            this.balanceAccountDtoBuilder = new BalanceAccountDtoBuilder();

            this.RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            this.RuleFor(x => x.Currency, fake => fake.Finance.Currency().Code);
            this.RuleFor(x => x.Amount, fake => decimal.Round(fake.Random.Decimal(0, 10000), 2));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
            this.RuleFor(x => x.Account, fake => balanceAccountDtoBuilder.Generate());
        }

        public BalanceDtoBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public BalanceDtoBuilder WithCurrency(string currency)
        {
            this.RuleFor(c => c.Currency, currency);
            return this;
        }

        public BalanceDtoBuilder WithAmount(decimal amount)
        {
            this.RuleFor(c => c.Amount, amount);
            return this;
        }

        public BalanceDtoBuilder WithAccount(BalanceAccountDto account)
        {
            this.RuleFor(x => x.Account, fake => account);
            return this;
        }

        public BalanceDtoBuilder WithActive(bool isActive)
        {
            this.RuleFor(c => c.IsActive, isActive);
            return this;
        }

        public BalanceDtoBuilder FromCreateDto(CreateBalanceDto balance)
        {
            this
                .WithName(balance.Name)
                .WithAccount(balanceAccountDtoBuilder.Generate())
                .WithCurrency(balance.Currency)
                .WithActive(balance.IsActive);
            return this;
        }

        public BalanceDtoBuilder FromUpdateDto(UpdateBalanceDto balance)
        {
            this
                .WithName(balance.Name)
                .WithActive(balance.IsActive);
            return this;
        }
    }
}
