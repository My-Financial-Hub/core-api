using System;
using NUnit.Framework;
using FinancialHub.Infra.Contexts;
using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialHub.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.Infra.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T> where T: BaseEntity
    {
        protected Random random;

        protected FinancialHubContext context;
        protected IBaseRepository<T> repository;

        [SetUp]
        protected virtual void Setup()
        {
            this.context = new FinancialHubContext(
                new DbContextOptionsBuilder<FinancialHubContext>().UseInMemoryDatabase($"Database_{Guid.NewGuid()}").Options
            );

            this.random = new Random();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            this.context.Dispose();
        }

        protected virtual async Task InsertData(ICollection<T> items)
        {
            await this.context.Set<T>().AddRangeAsync(items);
            await this.context.SaveChangesAsync();
        }

        protected virtual ICollection<T> GenerateData(int min = 1,int max = 100, params object[] props)
        {
            var count = random.Next(min, max);
            var data = new T[count];

            for (int i = 0; i < count; i++)
            {
                data[i] = this.GenerateObject(Guid.NewGuid(), props);
            }

            return data;
        }

        protected abstract T GenerateObject(Guid? id = null, params object[] props);
    }
}
