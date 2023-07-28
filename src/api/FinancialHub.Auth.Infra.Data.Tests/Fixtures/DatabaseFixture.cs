using FinancialHub.Auth.Infra.Data.Contexts;
using Microsoft.Data.Sqlite;
using FinancialHub.Domain.Entities;
using System.Collections;

namespace FinancialHub.Auth.Infra.Data.Tests.Fixtures
{
    public class DatabaseFixture : IEnumerable, IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerator GetEnumerator()
        {
            yield return this;
        }

        protected FinancialHubAuthContext context;
        public FinancialHubAuthContext Context => context;

        protected Random random;
        public Random Random => random;

        protected static FinancialHubAuthContext GetContext()
        {
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();
            var cfg = new DbContextOptionsBuilder<FinancialHubAuthContext>().UseSqlite(conn);
            cfg.EnableSensitiveDataLogging(true);

            return new FinancialHubAuthContext(
                cfg.Options
            );
        }

        public async Task<T> InsertData<T>(T item) where T: BaseEntity
        {
            var res = await this.context.Set<T>().AddAsync(item);
            item.Id = res.Entity.Id;
            await this.context.SaveChangesAsync();
            res.State = EntityState.Detached;

            return res.Entity;
        }

        public async Task<IEnumerable<T>> InsertData<T>(IEnumerable<T> items) where T : BaseEntity
        {
            await this.context.Set<T>().AddRangeAsync(items);
            await this.context.SaveChangesAsync();

            return items;
        }

        public void SetUp()
        {
            this.context = GetContext();
            this.context.Database.EnsureCreated();
            this.random = new Random();
        }

        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
            this.context.Database.CloseConnection();
            this.Dispose();
        }
    }
}
