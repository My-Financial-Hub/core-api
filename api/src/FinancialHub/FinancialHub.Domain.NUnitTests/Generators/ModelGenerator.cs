using Bogus;
using FinancialHub.Domain.Enums;
using FinancialHub.Domain.Models;
using System;

namespace FinancialHub.Domain.NUnitTests.Generators
{
    public class ModelGenerator
    {
        private readonly Random random;

        public ModelGenerator(Random random)
        {
            this.random = random;
        }

        public AccountModel GenerateAccount(Guid? id = null)
        {
            var fake = new Faker<AccountModel>();

            fake.RuleFor(x => x.Id, fake => id ?? fake.Database.Random.Guid())
                .RuleFor(x => x.Name, fake => fake.Finance.AccountName())
                .RuleFor(x => x.Currency, fake => fake.Finance.Currency().Code)
                .RuleFor(x => x.Description, fake => fake.Lorem.Sentences(random.Next(1, 5)))
                .RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());

            return fake.Generate();
        }

        public CategoryModel GenerateCategory(Guid? id = null)
        {
            var fake = new Faker<CategoryModel>();

            fake.RuleFor(x => x.Id, fake => id ?? fake.Database.Random.Guid())
                .RuleFor(x => x.Name, fake => fake.Finance.Random.Word())
                .RuleFor(x => x.Description, fake => fake.Lorem.Sentences(random.Next(1, 5)))
                .RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());

            return fake.Generate();
        }

        public TransactionModel GenerateTransaction(Guid? id = null, Guid? accountId = null, Guid? categoryId = null)
        {
            var account = this.GenerateAccount(accountId);
            var category = this.GenerateCategory(categoryId);

            var fake = new Faker<TransactionModel>();

            fake.RuleFor(x => x.Id, fake => id ?? fake.Database.Random.Guid())
                .RuleFor(x => x.Amount, fake => fake.Random.Decimal(0, 10000))
                .RuleFor(x => x.Description, fake => fake.Lorem.Sentences(random.Next(1, 5)))
                .RuleFor(x => x.IsActive, fake => fake.System.Random.Bool())
                .RuleFor(x => x.Type, fake => fake.PickRandom<TransactionType>())
                .RuleFor(x => x.Status, fake => fake.PickRandom<TransactionStatus>())
                .RuleFor(x => x.AccountId, fake => account.Id)
                .RuleFor(x => x.Account, fake => account)
                .RuleFor(x => x.CategoryId, fake => category.Id)
                .RuleFor(x => x.Category, fake => category)
                .RuleFor(x => x.TargetDate, fake => fake.Date.RecentOffset())
                .RuleFor(x => x.FinishDate, fake => fake.Date.RecentOffset());

            return fake.Generate();
        }
    }
}
