using System.Net;
using NUnit.Framework;
using System.Threading.Tasks;
using FinancialHub.IntegrationTests.Base;
using FinancialHub.Domain.Responses.Success;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Tests.Builders.Entities;

namespace FinancialHub.IntegrationTests.Accounts
{
    public class AccountsControllerTests : BaseControllerTests
    {
        private readonly AccountEntityBuilder builder;

        public AccountsControllerTests() : base()
        {
            this.builder = new AccountEntityBuilder();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Accounts_Get_ShouldReturnAccounts()
        {
            this.database.Accounts.Add(this.builder.Generate());
            await this.database.SaveChangesAsync();
            var response = await this.client.GetAsync("/accounts");

            var result = await ReadContentAsync<ListResponse<AccountModel>>(response.Content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}