using Bogus;
using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances
{
    public class BalanceAccountDtoBuilder : Faker<BalanceAccountDto>
    {
        public BalanceAccountDtoBuilder() : base()
        {
            RuleFor(x => x.Id, fake => fake.Random.Uuid());
            RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public BalanceAccountDtoBuilder WithId(Guid id)
        {
            RuleFor(c => c.Id, id);
            return this;
        }

        public BalanceAccountDtoBuilder WithName(string name)
        {
            RuleFor(c => c.Name, name);
            return this;
        }

        public BalanceAccountDtoBuilder WithDescription(string description)
        {
            RuleFor(c => c.Description, description);
            return this;
        }

        public BalanceAccountDtoBuilder WithActive(bool isActive)
        {
            RuleFor(c => c.IsActive, isActive);
            return this;
        }
    }
}
