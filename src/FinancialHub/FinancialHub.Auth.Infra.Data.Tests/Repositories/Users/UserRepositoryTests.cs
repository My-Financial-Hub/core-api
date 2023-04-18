using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Infra.Data.Contexts;
using FinancialHub.Auth.Infra.Data.Repositories;
using FinancialHub.Auth.Tests.Common.Builders.Entities;

namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        private FinancialHubAuthContext context;
        private IUserRepository repository;
        private UserEntityBuilder builder;

        protected static FinancialHubAuthContext GetContext()
        {
            var cfg = new DbContextOptionsBuilder<FinancialHubAuthContext>().UseSqlServer("Server=localhost,1450;Database=financial_hub;user=sa;pwd=P@ssw0rd!");
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
    }
}
