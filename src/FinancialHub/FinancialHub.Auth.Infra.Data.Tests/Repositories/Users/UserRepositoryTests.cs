using Microsoft.EntityFrameworkCore;
using FinancialHub.Auth.Tests.Commom.Builders.Entities;
using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Infra.Data.Contexts;
using FinancialHub.Auth.Infra.Data.Repositories;

namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        private FinancialHubAuthContext context;
        private IUserRepository repository;
        private UserEntityBuilder builder;

        protected FinancialHubAuthContext GetContext()
        {
            var cfg = new DbContextOptionsBuilder<FinancialHubAuthContext>().UseSqlServer("Server=localhost,1450;Database=financial_hub;user=sa;pwd=P@ssw0rd!;");
            cfg.EnableSensitiveDataLogging(true);

            return new FinancialHubAuthContext(
                cfg.Options
            );
        }

        [SetUp]
        public void SetUp()
        {
            this.builder = new UserEntityBuilder();
            this.context = this.GetContext();
            this.repository = new UserRepository(this.context);
        }

        protected virtual void AssertCreated(UserEntity createdItem)
        {
            Assert.Multiple(() =>
            {
                Assert.That(createdItem, Is.Not.Null);
                Assert.That(createdItem.Id, Is.Not.Null);
                Assert.That(createdItem.FirstName, Is.Not.Null);
                Assert.That(createdItem.Email, Is.Not.Null);
                Assert.That(createdItem.Password, Is.Not.Null);
                Assert.That(createdItem.CreationTime, Is.Not.Null);
                Assert.That(createdItem.UpdateTime, Is.Not.Null);

                Assert.That(context.Users.ToList(), Is.Not.Empty);
            });
        }
    }
}
