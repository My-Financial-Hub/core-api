using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Accounts;

namespace FinancialHub.Core.IntegrationTests.Controllers
{
    public partial class AccountsControllerTests
    {
        private CreateAccountDtoBuilder createAccountDtoBuilder;
        protected void AddCreateAccountBuilder()
        {
            createAccountDtoBuilder = new CreateAccountDtoBuilder();
        }

        [Test]
        public async Task Post_ValidAccount_ReturnCreatedAccount()
        {
            var data = createAccountDtoBuilder.Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<AccountDto>>();
            var resultData = result?.Data;
            Assert.That(resultData, Is.Not.Null);
            Assert.AreEqual(data.Name, resultData?.Name);
            Assert.AreEqual(data.Description, resultData?.Description);
            Assert.AreEqual(data.IsActive, resultData?.IsActive);
        }

        [Test]
        public async Task Post_ValidAccount_CreateAccount()
        {
            var body = createAccountDtoBuilder.Generate();

            await client.PostAsync(baseEndpoint, body);

            var account = fixture
                .GetData<AccountEntity>()
                .FirstOrDefault(
                    acc =>
                        acc.Name == body.Name &&
                        acc.Description == body.Description &&
                        acc.IsActive == body.IsActive
                );
            Assert.NotNull(account);
        }

        [Test]
        public async Task Post_ValidAccount_CreateDefaultBalance()
        {
            var body = createAccountDtoBuilder.Generate();

            await client.PostAsync(baseEndpoint, body);

            var account = fixture.GetData<AccountEntity>().First();
            var balances = fixture.GetData<BalanceEntity>();

            Assert.IsNotNull(balances.FirstOrDefault(x => x.AccountId == account.Id && x.Name == $"{account.Name} Default Balance"));
        }
    }
}
