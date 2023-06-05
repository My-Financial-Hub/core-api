using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Infra.Data.Contexts;
using FinancialHub.Auth.Infra.Data.Repositories;
using FinancialHub.Auth.Tests.Common.Builders.Entities;
using FinancialHub.Domain.Entities;
using Microsoft.Data.Sqlite;

namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        private FinancialHubAuthContext context;
        private IUserRepository repository;
        private UserEntityBuilder builder;

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

        [SetUp]
        public void SetUp()
        {
            this.builder = new UserEntityBuilder();
            this.context = GetContext();
            this.context.Database.EnsureCreated();
            this.repository = new UserRepository(this.context);
        }

        protected virtual void AssertCreated(UserEntity createdItem)
        {
            Assert.Multiple(() =>
            {
                Assert.That(context.Users.ToList(), Is.Not.Empty);

                var datebaseUser = context.Users.First(u => u.Id == createdItem.Id);
                Assert.That(datebaseUser, Is.EqualTo(createdItem));
            });
        }

        protected virtual async Task<ICollection<UserEntity>> InsertData(ICollection<UserEntity> items)
        {
            var list = new List<UserEntity>();
            foreach (var item in items)
            {
                var entity = await this.context.Users.AddAsync(item);
                await this.context.SaveChangesAsync();
                list.Add(entity.Entity);
            }
            await this.context.SaveChangesAsync();
            return list;
        }

        protected virtual async Task<UserEntity> InsertData(UserEntity item)
        {
            var res = await this.context.Users.AddAsync(item);
            item.Id = res.Entity.Id;
            await this.context.SaveChangesAsync();
            res.State = EntityState.Detached;

            return res.Entity;
        }
    }
}
