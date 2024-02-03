using Bogus;
using FinancialHub.Core.Domain.DTOS.Transactions;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions
{
    public class TransactionCategoryDtoBuilder : Faker<TransactionCategoryDto>
    {
        public TransactionCategoryDtoBuilder() : base()
        {
            this.RuleFor(x => x.Id, fake => fake.Random.Uuid());
            this.RuleFor(x => x.Name, fake => fake.Finance.Random.Word());
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public TransactionCategoryDtoBuilder WithId(Guid guid)
        {
            this.RuleFor(x => x.Id, guid);
            return this;
        }

        public TransactionCategoryDtoBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public TransactionCategoryDtoBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public TransactionCategoryDtoBuilder WithActive(bool isActive)
        {
            this.RuleFor(c => c.IsActive, isActive);
            return this;
        }
    }
}
