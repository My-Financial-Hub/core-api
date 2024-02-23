using Bogus;
using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Accounts
{
    public class CreateAccountDtoBuilder: Faker<CreateAccountDto>
    {
        public CreateAccountDtoBuilder() : base()
        {
            RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public CreateAccountDtoBuilder WithName(string name)
        {
            RuleFor(c => c.Name, name);
            return this;
        }

        public CreateAccountDtoBuilder WithDescription(string description)
        {
            RuleFor(c => c.Description, description);
            return this;
        }

        public CreateAccountDtoBuilder WithActive(bool isActive)
        {
            RuleFor(c => c.IsActive, isActive);
            return this;
        }
    }
}
