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

        public void SetUp()
        {
            this.context = GetContext();
            this.context.Database.EnsureCreated();
        }

        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
            this.context.Database.CloseConnection();
            this.Dispose();
        }
    }
}
