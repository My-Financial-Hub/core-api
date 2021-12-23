using Bogus;
using System;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Enums;

namespace FinancialHub.Infra.Data.NUnitTests.Generators
{
    public class EntityGenerator
    {
        private readonly Random random;

        public EntityGenerator(Random random)
        {
            this.random = random; 
        }

        public AccountEntity GenerateAccount(Guid? id = null)
        {
            var fake = new Faker<AccountEntity>();

            fake.RuleFor(x => x.Id,             fake => id ?? fake.Database.Random.Guid())
                .RuleFor(x => x.Name,           fake => fake.Finance.AccountName())
                .RuleFor(x => x.Currency,       fake => fake.Finance.Currency().Code)
                .RuleFor(x => x.Description,    fake => fake.Lorem.Sentences(random.Next(1,5)))
                .RuleFor(x => x.IsActive,       fake => fake.System.Random.Bool())
                .RuleFor(x => x.CreationTime,   fake => DateTimeOffset.Now)
                .RuleFor(x => x.UpdateTime,     fake => DateTimeOffset.Now);

            return fake.Generate();
        }

        public CategoryEntity GenerateCategory(Guid? id = null)
        {
            var fake = new Faker<CategoryEntity>();

            fake.RuleFor(x => x.Id,             fake => id ?? fake.Database.Random.Guid())
                .RuleFor(x => x.Name,           fake => fake.Finance.Random.Word())
                .RuleFor(x => x.Description,    fake => fake.Lorem.Sentences(random.Next(1, 5)))
                .RuleFor(x => x.IsActive,       fake => fake.System.Random.Bool())
                .RuleFor(x => x.CreationTime,   fake => DateTimeOffset.Now)
                .RuleFor(x => x.UpdateTime,     fake => DateTimeOffset.Now);

            return fake.Generate();
        }

        public TransactionEntity GenerateTransaction(Guid? id = null, Guid? accountId = null, Guid? categoryId = null)
        {
            var account     = this.GenerateAccount(accountId);
            var category    = this.GenerateCategory(categoryId);

            var fake = new Faker<TransactionEntity>();

            fake.RuleFor(x => x.Id,             fake => id ?? fake.Database.Random.Guid())
                .RuleFor(x => x.Amount ,        fake => fake.Random.Decimal(0,10000))
                .RuleFor(x => x.Description,    fake => fake.Lorem.Sentences(random.Next(1, 5)))
                .RuleFor(x => x.IsActive,       fake => fake.System.Random.Bool())
                .RuleFor(x => x.Type,           fake => fake.PickRandom<TransactionType>())  
                .RuleFor(x => x.Status ,        fake => fake.PickRandom<TransactionStatus>())
                .RuleFor(x => x.AccountId,      fake => account.Id)
                .RuleFor(x => x.Account,        fake => account)
                .RuleFor(x => x.CategoryId,     fake => category.Id)
                .RuleFor(x => x.Category,       fake => category)
                .RuleFor(x => x.TargetDate,     fake => fake.Date.RecentOffset())
                .RuleFor(x => x.FinishDate,     fake => fake.Date.RecentOffset())
                .RuleFor(x => x.CreationTime,   fake => DateTimeOffset.Now)
                .RuleFor(x => x.UpdateTime,     fake => DateTimeOffset.Now);

            return fake.Generate();
        }
    }
}
