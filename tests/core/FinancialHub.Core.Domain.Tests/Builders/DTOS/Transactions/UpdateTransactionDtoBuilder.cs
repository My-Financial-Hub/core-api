using Bogus;
using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Enums;
using FinancialHub.Core.Domain.Tests.Builders.Models;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions
{
    public class UpdateTransactionDtoBuilder : Faker<UpdateTransactionDto>
    {
        public UpdateTransactionDtoBuilder() : base()
        {
            var balance = new BalanceModelBuilder().Generate();
            var category = new CategoryModelBuilder().Generate();

            this.RuleFor(x => x.Amount, fake => decimal.Round(fake.Random.Decimal(0, 10000), 2));
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
            this.RuleFor(x => x.Type, fake => fake.PickRandom<TransactionType>());
            this.RuleFor(x => x.Status, fake => fake.PickRandom<TransactionStatus>());

            this.RuleFor(x => x.BalanceId, fake => balance.Id);

            this.RuleFor(x => x.CategoryId, fake => category.Id);

        }

        public UpdateTransactionDtoBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public UpdateTransactionDtoBuilder WithCategoryId(Guid? categoryId)
        {
            this.RuleFor(x => x.CategoryId, fake => categoryId);
            return this;
        }

        public UpdateTransactionDtoBuilder WithBalanceId(Guid? balanceId)
        {
            this.RuleFor(x => x.BalanceId, fake => balanceId);
            return this;
        }

        public UpdateTransactionDtoBuilder WithStatus(TransactionStatus transactionStatus)
        {
            this.RuleFor(x => x.Status, fake => transactionStatus);
            return this;
        }

        public UpdateTransactionDtoBuilder WithAmount(decimal amount)
        {
            this.RuleFor(x => x.Amount, fake => amount);
            return this;
        }

        public UpdateTransactionDtoBuilder WithType(TransactionType type)
        {
            this.RuleFor(x => x.Type, fake => type);
            return this;
        }

        public UpdateTransactionDtoBuilder WithActiveStatus(bool isActive)
        {
            this.RuleFor(x => x.IsActive, fake => isActive);
            return this;
        }
    }
}
