using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using FinancialHub.Common.Entities;
using FinancialHub.Common.Tests.Builders.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Data.Contexts;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T>
        where T : BaseEntity
    {
        protected BaseEntityBuilder<T> builder;
        protected FinancialHubContext context;
        protected Random random;

        protected IBaseRepository<T> repository;

        protected FinancialHubContext GetContext()
        {
            //TODO: use docker/ local sql database
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();

            var cfg = new DbContextOptionsBuilder<FinancialHubContext>().UseSqlite(conn);
            cfg.EnableSensitiveDataLogging(true);

            return new FinancialHubContext(
                cfg.Options
            );
        }

        #region lifeCycle
        [SetUp]
        protected virtual void Setup()
        {
            this.random = new Random();
            this.context = this.GetContext();
            this.context.Database.EnsureCreated();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            this.context.Dispose();
        }
        #endregion

        #region Generics
        protected virtual async Task<ICollection<Y>> InsertData<Y>(ICollection<Y> items)
             where Y : BaseEntity
        {
            var list = new List<Y>();
            foreach (var item in items)
            {
                var entity = await this.context.Set<Y>().AddAsync(item);
                await this.context.SaveChangesAsync();
                list.Add(entity.Entity);
            }
            await this.context.SaveChangesAsync();
            return list;
        }

        protected virtual async Task<Y> InsertData<Y>(Y item)
             where Y : BaseEntity
        {
            var res = await this.context.Set<Y>().AddAsync(item);
            item.Id = res.Entity.Id;
            await this.context.SaveChangesAsync();

            return res.Entity;
        }

        #endregion

        protected virtual ICollection<T> GenerateData(int min = 1, int max = 100)
        {
            var count = new Random().Next(min, max);
            return this.builder.Generate(count);
        }

        protected virtual T GenerateObject(Guid? id = null)
        {
            if (id == null)
            {
                return this.builder.Generate();
            }
            else
            {
                return this.builder.WithId(id.Value).Generate();
            }
        }
    }
}
