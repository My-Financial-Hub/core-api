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
        public async Task Post_InvalidAccount_Returns400BadRequest()
        {
            var data = createAccountDtoBuilder
                .WithName(string.Empty)
                .WithDescription(new string('o', 501))
                .Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Post_InvalidAccount_ReturnsAccountValidationError()
        {
            var data = createAccountDtoBuilder
                .WithName(string.Empty)
                .WithDescription(new string('o', 501))
                .Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            var validationResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

            Assert.IsNotNull(validationResponse);
            Assert.AreEqual(2, validationResponse!.Errors.Length);
        }

        [Test]
        public async Task Post_ValidAccount_ReturnsCreatedAccount()
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
        public async Task Post_ValidAccount_ReturnsCreatedAccountId()
        {
            var data = createAccountDtoBuilder.Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<AccountDto>>();
            var account = fixture
                .GetData<AccountEntity>()
                .First(
                    acc =>
                        acc.Name == data.Name &&
                        acc.Description == data.Description &&
                        acc.IsActive == data.IsActive
                );
            Assert.AreEqual(account.Id, result?.Data?.Id);
        }

        [Test]
        public async Task Post_ValidAccount_CreatesAccount()
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
        public async Task Post_ValidAccount_CreatesDefaultBalance()
        {
            var body = createAccountDtoBuilder.Generate();

            await client.PostAsync(baseEndpoint, body);

            var account = fixture.GetData<AccountEntity>().First();
            var balances = fixture.GetData<BalanceEntity>();

            Assert.IsNotNull(balances.FirstOrDefault(x => x.AccountId == account.Id && x.Name == $"{account.Name} Default Balance"));
        }
    }
}
