using Bogus;
using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Accounts
{
    public class AccountDtoBuilder : Faker<AccountDto>
    {
        public AccountDtoBuilder() : base()
        {
            RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public AccountDtoBuilder WithName(string name)
        {
            RuleFor(c => c.Name, name);
            return this;
        }

        public AccountDtoBuilder WithDescription(string description)
        {
            RuleFor(c => c.Description, description);
            return this;
        }

        public AccountDtoBuilder WithActive(bool isActive)
        {
            RuleFor(c => c.IsActive, isActive);
            return this;
        }

        public AccountDtoBuilder FromCreateDto(CreateAccountDto account)
        {
            this
                .WithName(account.Name)
                .WithDescription(account.Description)
                .WithActive(account.IsActive);
            return this;
        }

        public AccountDtoBuilder FromUpdateDto(UpdateAccountDto account)
        {
            this
                .WithName(account.Name)
                .WithDescription(account.Description)
                .WithActive(account.IsActive);
            return this;
        }
    }
}
