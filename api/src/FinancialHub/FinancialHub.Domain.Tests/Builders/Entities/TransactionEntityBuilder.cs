using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Enums;
using System;

namespace FinancialHub.Domain.Tests.Builders.Entities
{
    public class TransactionEntityBuilder : BaseEntityBuilder<TransactionEntity>
    {
        public TransactionEntityBuilder() : base()
        {
            var balance     = new BalanceEntityBuilder().Generate();
            var category    = new CategoryEntityBuilder().Generate();

            this.RuleFor(x => x.Amount, fake => decimal.Round(fake.Random.Decimal(0, 10000),2));
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
            this.RuleFor(x => x.Type, fake => fake.PickRandom<TransactionType>());
            this.RuleFor(x => x.Status, fake => fake.PickRandom<TransactionStatus>());

            this.RuleFor(x => x.BalanceId, fake => balance.Id);
            this.RuleFor(x => x.Balance, fake => balance);
            this.RuleFor(x => x.CategoryId, fake => category.Id);
            this.RuleFor(x => x.Category, fake => category);

            this.RuleFor(x => x.TargetDate, fake => fake.Date.RecentOffset());
            this.RuleFor(x => x.FinishDate, fake => fake.Date.RecentOffset());
        }

        public TransactionEntityBuilder WithCategoryId(Guid? categoryId)
        {
            this.RuleFor(x => x.CategoryId, fake => categoryId);
            this.RuleFor(x => x.Category, fake => default);
            return this;
        }

        public TransactionEntityBuilder WithCategory(CategoryEntity category)
        {
            this.WithCategoryId(category.Id);
            this.RuleFor(x => x.Category, fake => category);
            return this;
        }

        public TransactionEntityBuilder WithBalance(BalanceEntity balance)
        {
            this.WithBalanceId(balance.Id);
            this.RuleFor(x => x.Balance, fake => balance);
            return this;
        }

        public TransactionEntityBuilder WithBalanceId(Guid? balanceId)
        {
            this.RuleFor(x => x.BalanceId, fake => balanceId);
            this.RuleFor(x => x.Balance, fake => default);
            return this;
        }

        public TransactionEntityBuilder WithStatus(TransactionStatus transactionStatus)
        {
            this.RuleFor(x => x.Status, fake => transactionStatus);
            return this;
        }

        public TransactionEntityBuilder WithAmount(decimal amount)
        {
            this.RuleFor(x => x.Amount, fake => amount);
            return this;
        }

        public TransactionEntityBuilder WithType(TransactionType type)
        {
            this.RuleFor(x => x.Type, fake => type);
            return this;
        }

        public TransactionEntityBuilder WithActiveStatus(bool isActive)
        {
            this.RuleFor(x => x.IsActive, fake => isActive);
            return this;
        }
    }
}
