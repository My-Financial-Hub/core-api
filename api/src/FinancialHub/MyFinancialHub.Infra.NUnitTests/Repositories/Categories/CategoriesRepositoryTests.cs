using System;
using System.Collections.Generic;
using FinancialHub.Domain.Entities;
using FinancialHub.Infra.NUnitTests.Repositories.Base;
using FinancialHub.Infra.Repositories;
using NUnit.Framework;

namespace FinancialHub.Infra.NUnitTests.Repositories.Categories
{
    public class CategoriesRepositoryTests : BaseRepositoryTests<CategoryEntity>
    {
        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            this.repository = new CategoriesRepository(this.context);
        }

        protected override ICollection<CategoryEntity> GenerateData(int min = 1, int max = 100, params object[] props)
        {
            var count = random.Next(min, max);
            var accounts = new CategoryEntity[count];

            for (int i = 0; i < count; i++)
            {
                accounts[i] = this.GenerateObject(Guid.NewGuid(), props);
            }

            return accounts;
        }

        protected override CategoryEntity GenerateObject(Guid? id = null, params object[] props)
        {
            var active = random.Next(0, 1);

            return new CategoryEntity()
            {
                Id = id,
                Name = props.Length > 0 ? props[0].ToString() : Guid.NewGuid().ToString(),
                Description = props.Length > 1 ? props[1].ToString() : Guid.NewGuid().ToString(),
                IsActive = props.Length > 3 ? (bool)props[3] : active == 1,
                CreationTime = DateTimeOffset.UtcNow,
                UpdateTime = DateTimeOffset.UtcNow,
            };
        }
    }
}
