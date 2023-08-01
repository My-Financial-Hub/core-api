using FinancialHub.Domain.Enums;

namespace FinancialHub.Domain.Tests.Builders.Models
{
    public class TransactionModelBuilder : BaseModelBuilder<TransactionModel>
    {
        public TransactionModelBuilder() : base()
        {
            var balance = new BalanceModelBuilder().Generate();
            var category = new CategoryModelBuilder().Generate();

            this.RuleFor(x => x.Amount, fake => decimal.Round(fake.Random.Decimal(0, 10000),2));
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
            this.RuleFor(x => x.Type, fake => fake.PickRandom<TransactionType>());
            this.RuleFor(x => x.Status, fake => fake.PickRandom<TransactionStatus>());

            this.RuleFor(x => x.BalanceId, fake => balance.Id);
            this.RuleFor(x => x.Balance, fake => balance);

            this.RuleFor(x => x.CategoryId, fake => category.Id);
            this.RuleFor(x => x.Category, fake => category);

        }

        public TransactionModelBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public TransactionModelBuilder WithCategoryId(Guid? categoryId)
        {
            this.RuleFor(x => x.CategoryId, fake => categoryId);
            this.Ignore( x => x.Category);
            return this;
        }

        public TransactionModelBuilder WithCategory(CategoryModel category)
        {
            this.WithCategoryId(category.Id);
            this.RuleFor(x => x.Category, fake => category);
            return this;
        }

        public TransactionModelBuilder WithBalance(BalanceModel balance)
        {
            this.WithBalanceId(balance.Id);
            this.RuleFor(x => x.Balance, fake => balance);
            return this;
        }

        public TransactionModelBuilder WithBalanceId(Guid? balanceId)
        {
            this.RuleFor(x => x.BalanceId, fake => balanceId);
            this.Ignore(x => x.Balance);
            return this;
        }

        public TransactionModelBuilder WithStatus(TransactionStatus transactionStatus)
        {
            this.RuleFor(x => x.Status, fake => transactionStatus);
            return this;
        }

        public TransactionModelBuilder WithAmount(decimal amount)
        {
            this.RuleFor(x => x.Amount, fake => amount);
            return this;
        }

        public TransactionModelBuilder WithType(TransactionType type)
        {
            this.RuleFor(x => x.Type, fake => type);
            return this;
        }

        public TransactionModelBuilder WithActiveStatus(bool isActive)
        {
            this.RuleFor(x => x.IsActive, fake => isActive);
            return this;
        }
    }
}
