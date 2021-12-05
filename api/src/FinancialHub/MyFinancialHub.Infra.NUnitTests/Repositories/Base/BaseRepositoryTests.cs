using System;
using NUnit.Framework;
using FinancialHub.Infra.Contexts;
using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialHub.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace FinancialHub.Infra.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T> where T : BaseEntity
    {
        protected Random random;

        protected FinancialHubContext context;
        protected IBaseRepository<T> repository;

        protected FinancialHubContext GetContext()
        {
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();

            var cfg = new DbContextOptionsBuilder<FinancialHubContext>().UseSqlite(conn);
            cfg.EnableSensitiveDataLogging(true);

            return new FinancialHubContext(
                cfg.Options
            );
        }

        [SetUp]
        protected virtual void Setup()
        {  
            this.context = this.GetContext();
            context.Database.EnsureCreated();

            this.random = new Random();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            this.context.Dispose();
        }

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

        protected virtual ICollection<T> GenerateData(int min = 1, int max = 100, params object[] props)
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
