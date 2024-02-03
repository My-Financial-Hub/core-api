using Bogus;
using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions
{
    public class CreateTransactionDtoBuilder : Faker<CreateTransactionDto>
    {
        public CreateTransactionDtoBuilder() : base()
        {
            this.RuleFor(x => x.Amount, fake => decimal.Round(fake.Random.Decimal(0, 10000), 2));
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
            this.RuleFor(x => x.Type, fake => fake.PickRandom<TransactionType>());
            this.RuleFor(x => x.Status, fake => fake.PickRandom<TransactionStatus>());
            this.RuleFor(x => x.BalanceId, fake => fake.Random.Uuid());
            this.RuleFor(x => x.CategoryId, fake => fake.Random.Uuid());
        }

        public CreateTransactionDtoBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public CreateTransactionDtoBuilder WithCategoryId(Guid? categoryId)
        {
            this.RuleFor(x => x.CategoryId, fake => categoryId);
            return this;
        }

        public CreateTransactionDtoBuilder WithBalanceId(Guid? balanceId)
        {
            this.RuleFor(x => x.BalanceId, fake => balanceId);
            return this;
        }

        public CreateTransactionDtoBuilder WithStatus(TransactionStatus transactionStatus)
        {
            this.RuleFor(x => x.Status, fake => transactionStatus);
            return this;
        }

        public CreateTransactionDtoBuilder WithAmount(decimal amount)
        {
            this.RuleFor(x => x.Amount, fake => amount);
            return this;
        }

        public CreateTransactionDtoBuilder WithType(TransactionType type)
        {
            this.RuleFor(x => x.Type, fake => type);
            return this;
        }

        public CreateTransactionDtoBuilder WithActiveStatus(bool isActive)
        {
            this.RuleFor(x => x.IsActive, fake => isActive);
            return this;
        }
    }
}
