using Bogus;
using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions
{
    public class TransactionDtoBuilder : Faker<TransactionDto>
    {
        private readonly TransactionBalanceDtoBuilder balanceDtoBuilder;
        private readonly TransactionCategoryDtoBuilder categoryDtoBuilder;
        public TransactionDtoBuilder() : base()
        {
            this.balanceDtoBuilder = new TransactionBalanceDtoBuilder();
            this.categoryDtoBuilder = new TransactionCategoryDtoBuilder();

            this.RuleFor(x => x.Id, fake => fake.Random.Uuid());
            this.RuleFor(x => x.Amount, fake => decimal.Round(fake.Random.Decimal(0, 10000), 2));
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
            this.RuleFor(x => x.Type, fake => fake.PickRandom<TransactionType>());
            this.RuleFor(x => x.Status, fake => fake.PickRandom<TransactionStatus>());
            this.RuleFor(x => x.Balance, fake => balanceDtoBuilder.Generate());
            this.RuleFor(x => x.Category, fake => categoryDtoBuilder.Generate());

        }

        public TransactionDtoBuilder WithId(Guid id)
        {
            this.RuleFor(c => c.Id, id);
            return this;
        }

        public TransactionDtoBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public TransactionDtoBuilder WithCategory(TransactionCategoryDto category)
        {
            this.RuleFor(x => x.Category, fake => category);
            return this;
        }

        public TransactionDtoBuilder WithBalance(TransactionBalanceDto balance)
        {
            this.RuleFor(x => x.Balance, fake => balance);
            return this;
        }

        public TransactionDtoBuilder WithStatus(TransactionStatus transactionStatus)
        {
            this.RuleFor(x => x.Status, fake => transactionStatus);
            return this;
        }

        public TransactionDtoBuilder WithAmount(decimal amount)
        {
            this.RuleFor(x => x.Amount, fake => amount);
            return this;
        }

        public TransactionDtoBuilder WithType(TransactionType type)
        {
            this.RuleFor(x => x.Type, fake => type);
            return this;
        }

        public TransactionDtoBuilder WithActiveStatus(bool isActive)
        {
            this.RuleFor(x => x.IsActive, fake => isActive);
            return this;
        }

        public TransactionDtoBuilder WithTargetDate(DateTimeOffset targetDate)
        {
            this.RuleFor(x => x.TargetDate, fake => targetDate);
            return this;
        }

        public TransactionDtoBuilder WithFinishDate(DateTimeOffset finishDate)
        {
            this.RuleFor(x => x.FinishDate, fake => finishDate);
            return this;
        }

        public TransactionDtoBuilder FromCreateDto(CreateTransactionDto transactionDto)
        {
            this
                .WithType(transactionDto.Type)
                .WithStatus(transactionDto.Status)
                .WithAmount(transactionDto.Amount)
                .WithDescription(transactionDto.Description)
                .WithTargetDate(transactionDto.TargetDate)
                .WithFinishDate(transactionDto.FinishDate)
                .WithCategory(categoryDtoBuilder.WithId(transactionDto.CategoryId))
                .WithBalance(balanceDtoBuilder.WithId(transactionDto.BalanceId))
                .WithActiveStatus(transactionDto.IsActive);
            return this;
        }

        public TransactionDtoBuilder FromUpdateDto(UpdateTransactionDto transactionDto)
        {
            this
                .WithType(transactionDto.Type)
                .WithStatus(transactionDto.Status)
                .WithAmount(transactionDto.Amount)
                .WithDescription(transactionDto.Description)
                .WithTargetDate(transactionDto.TargetDate)
                .WithFinishDate(transactionDto.FinishDate)
                .WithCategory(categoryDtoBuilder.WithId(transactionDto.CategoryId))
                .WithBalance(balanceDtoBuilder.WithId(transactionDto.BalanceId))
                .WithActiveStatus(transactionDto.IsActive);
            return this;
        }
    }
}
