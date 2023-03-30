using Microsoft.EntityFrameworkCore;
using FinancialHub.Auth.Infra.Data.Contexts;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Tests.Commom.Builders.Entities;
using FinancialHub.Auth.Domain.Entities;

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
            //this.repository = new IUserRepository();
            this.builder = new UserEntityBuilder();
            this.context = this.GetContext();
        }

        protected virtual void AssertCreated(UserEntity createdItem)
        {
            Assert.IsNotNull(createdItem);
            Assert.IsNotNull(createdItem.Id);
            Assert.IsNotNull(createdItem.FirstName);
            Assert.IsNotNull(createdItem.Email);
            Assert.IsNotNull(createdItem.Password);
            Assert.IsNotNull(createdItem.CreationTime);
            Assert.IsNotNull(createdItem.UpdateTime);

            Assert.IsNotEmpty(context.Users.ToList());
        }
    }
}
