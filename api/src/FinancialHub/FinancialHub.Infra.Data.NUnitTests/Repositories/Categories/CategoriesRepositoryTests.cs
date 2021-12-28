using System;
using FinancialHub.Domain.Entities;
using FinancialHub.Infra.Data.NUnitTests.Repositories.Base;
using FinancialHub.Infra.Data.Repositories;
using NUnit.Framework;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories
{
    public class CategoriesRepositoryTests : BaseRepositoryTests<CategoryEntity>
    {
        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            this.repository = new CategoriesRepository(this.context);
        }

        protected override CategoryEntity GenerateObject(Guid? id = null)
        {
            return this.generator.GenerateCategory(id);
        }
    }
}
