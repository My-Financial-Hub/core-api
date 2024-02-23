using Bogus;
using FinancialHub.Core.Domain.DTOS.Transactions;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions
{
    public class TransactionAccountDtoBuilder : Faker<TransactionAccountDto>
    {
        public TransactionAccountDtoBuilder() : base()
        {
            this.RuleFor(x => x.Id, fake => fake.Random.Uuid());
            this.RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public TransactionAccountDtoBuilder WithId(Guid guid)
        {
            this.RuleFor(x => x.Id, guid);
            return this;
        }

        public TransactionAccountDtoBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public TransactionAccountDtoBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public TransactionAccountDtoBuilder WithActive(bool isActive)
        {
            this.RuleFor(c => c.IsActive, isActive);
            return this;
        }
    }
}
