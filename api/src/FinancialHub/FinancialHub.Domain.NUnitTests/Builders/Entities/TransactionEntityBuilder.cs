using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Enums;

namespace FinancialHub.Domain.Tests.Builders.Entities
{
    public class TransactionEntityBuilder : BaseEntityBuilder<TransactionEntity>
    {
        private readonly AccountEntityBuilder accountEntityBuilder;
        private readonly CategoryEntityBuilder categoryEntityBuilder;

        public TransactionEntityBuilder() : this(new AccountEntityBuilder(),new CategoryEntityBuilder()){ }

        public TransactionEntityBuilder(
            AccountEntityBuilder accountEntityBuilder, CategoryEntityBuilder categoryEntityBuilder
        ) : base()
        {
            this.accountEntityBuilder = accountEntityBuilder;
            this.categoryEntityBuilder = categoryEntityBuilder;

            var account     = this.accountEntityBuilder.Generate();
            var category    = this.categoryEntityBuilder.Generate();

            this.RuleFor(x => x.Amount, fake => fake.Random.Decimal(0, 10000));
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
            this.RuleFor(x => x.Type, fake => fake.PickRandom<TransactionType>());
            this.RuleFor(x => x.Status, fake => fake.PickRandom<TransactionStatus>());

            this.RuleFor(x => x.AccountId, fake => account.Id);
            this.RuleFor(x => x.Account, fake => account);
            this.RuleFor(x => x.CategoryId, fake => category.Id);
            this.RuleFor(x => x.Category, fake => category);

            this.RuleFor(x => x.TargetDate, fake => fake.Date.RecentOffset());
            this.RuleFor(x => x.FinishDate, fake => fake.Date.RecentOffset());
        }

        public TransactionEntityBuilder WithCategory(CategoryEntity category)
        {
            this.RuleFor(x => x.CategoryId, fake => category.Id);
            this.RuleFor(x => x.Category, fake => category);

            return this;
        }

        public TransactionEntityBuilder WithAccount(AccountEntity account)
        {
            this.RuleFor(x => x.AccountId, fake => account.Id);
            this.RuleFor(x => x.Account, fake => account);

            return this;
        }
    }
}
