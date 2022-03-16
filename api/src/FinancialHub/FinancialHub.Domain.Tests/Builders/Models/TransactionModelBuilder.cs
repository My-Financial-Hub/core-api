using FinancialHub.Domain.Enums;
using FinancialHub.Domain.Models;

namespace FinancialHub.Domain.Tests.Builders.Models
{
    public class TransactionModelBuilder : BaseModelBuilder<TransactionModel>
    {
        public TransactionModelBuilder() : base()
        {
            var account = new AccountModelBuilder().Generate();
            var category = new CategoryModelBuilder().Generate();

            this.RuleFor(x => x.Amount, fake => fake.Random.Decimal(0, 10000));
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
            this.RuleFor(x => x.Type, fake => fake.PickRandom<TransactionType>());
            this.RuleFor(x => x.Status, fake => fake.PickRandom<TransactionStatus>());
            this.RuleFor(x => x.AccountId, fake => account.Id);
            this.RuleFor(x => x.Account, fake => account);
            this.RuleFor(x => x.CategoryId, fake => category.Id);
            this.RuleFor(x => x.Category, fake => category);

        }

        public TransactionModelBuilder WithCategory(CategoryModel category)
        {
            this.RuleFor(x => x.CategoryId, fake => category.Id);
            this.RuleFor(x => x.Category, fake => category);

            return this;
        }

        public TransactionModelBuilder WithAccount(AccountModel account)
        {
            this.RuleFor(x => x.AccountId, fake => account.Id);
            this.RuleFor(x => x.Account, fake => account);

            return this;
        }
    }
}
