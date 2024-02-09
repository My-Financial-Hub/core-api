using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.IntegrationTests.Controllers.Accounts
{
    public partial class AccountsControllerTests
    {
        [Test]
        public async Task GetAll_ReturnAccounts()
        {
            var data = entityBuilder.Generate(10);
            fixture.AddData(data.ToArray());

            var response = await client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<AccountDto>>();
            Assert.AreEqual(data.Count, result?.Data.Count);
        }

        [Test]
        public async Task GetAll_NoAccounts_ReturnEmptyList()
        {
            var response = await client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<AccountDto>>();
            Assert.IsEmpty(result?.Data);
        }
    }
}
