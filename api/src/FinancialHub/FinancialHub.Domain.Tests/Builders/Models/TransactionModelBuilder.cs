using FinancialHub.Domain.Enums;
using FinancialHub.Domain.Models;
using System;

namespace FinancialHub.Domain.Tests.Builders.Models
{
    public class TransactionModelBuilder : BaseModelBuilder<TransactionModel>
    {
        public TransactionModelBuilder() : base()
        {
            var account = new AccountModelBuilder().Generate();
            var category = new CategoryModelBuilder().Generate();

            this.RuleFor(x => x.Amount, fake => decimal.Round(fake.Random.Decimal(0, 10000),2));
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
            this.RuleFor(x => x.Type, fake => fake.PickRandom<TransactionType>());
            this.RuleFor(x => x.Status, fake => fake.PickRandom<TransactionStatus>());
            this.RuleFor(x => x.AccountId, fake => account.Id);
            this.RuleFor(x => x.Account, fake => account);
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
            this.RuleFor(x => x.Category, fake => default);
            return this;
        }

        public TransactionModelBuilder WithCategory(CategoryModel category)
        {
            this.WithCategoryId(category.Id);
            this.RuleFor(x => x.Category, fake => category);
            return this;
        }

        public TransactionModelBuilder WithAccount(AccountModel account)
        {
            this.WithAccountId(account.Id);
            this.RuleFor(x => x.Account, fake => account);
            return this;
        }

        public TransactionModelBuilder WithAccountId(Guid? accountId)
        {
            this.RuleFor(x => x.AccountId, fake => accountId);
            this.RuleFor(x => x.Account, fake => default);
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
