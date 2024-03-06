using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances;

namespace FinancialHub.Core.IntegrationTests.Controllers.Balances
{
    public partial class BalancesControllerTests
    {
        private CreateBalanceDtoBuilder createBalanceDtoBuilder;
        protected void AddCreateBalanceBuilder()
        {
            createBalanceDtoBuilder = new CreateBalanceDtoBuilder();
        }

        protected BalanceEntity GetBalance(CreateBalanceDto balance)
        {
            return fixture
                .GetData<BalanceEntity>()
                .First(
                    bal =>
                        bal.Name        == balance.Name &&
                        bal.Currency    == balance.Currency &&
                        bal.AccountId   == balance.AccountId &&
                        bal.IsActive    == balance.IsActive
                );
        }

        [Test]
        public async Task Post_InvalidBalance_Returns400BadRequest()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var body = createBalanceDtoBuilder
                .WithName(string.Empty)
                .WithCurrency(new string('o', 51))
                .WithAccountId(account.Id)
                .Generate();

            var response = await client.PostAsync(baseEndpoint, body);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Post_InvalidBalance_ReturnsBalanceValidationError()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var body = createBalanceDtoBuilder
                .WithName(string.Empty)
                .WithCurrency(new string('o', 51))
                .WithAccountId(account.Id)
                .Generate();

            var response = await client.PostAsync(baseEndpoint, body);
            var validationResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

            Assert.IsNotNull(validationResponse);
            Assert.AreEqual(2, validationResponse!.Errors.Length);
        }

        [Test]
        public async Task Post_BalanceWithInvalidAccount_Returns404NotFound()
        {
            var id = Guid.NewGuid();
            var data = createBalanceDtoBuilder.WithAccountId(id).Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual(result?.Message, $"Not found Account with id {id}");
        }

        [Test]
        public async Task Post_BalanceWithInvalidAccount_DoesNotCreateBalance()
        {
            var id = Guid.NewGuid();
            var data = createBalanceDtoBuilder.WithAccountId(id).Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var balance = fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.AccountId == id);
            Assert.IsNull(balance);
        }

        [Test]
        public async Task Post_ValidBalance_ReturnsCreatedBalance()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);
            var body = createBalanceDtoBuilder.WithAccountId(account.Id).Generate();

            var response = await client.PostAsync(baseEndpoint, body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceDto>>();
            var resultData = result?.Data;
            Assert.IsNotNull(resultData);
            Assert.AreEqual(body.Name, resultData?.Name);
            Assert.AreEqual(body.Currency, resultData?.Currency);
        }

        [Test]
        public async Task Post_ValidBalance_ReturnCreatedBalanceId()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);
            var body = createBalanceDtoBuilder.WithAccountId(account.Id).Generate();

            var response = await client.PostAsync(baseEndpoint, body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceDto>>();
            var balance = this.GetBalance(body);
            Assert.AreEqual(balance.Id, result?.Data?.Id);
        }

        [Test]
        public async Task Post_ValidBalance_CreatesBalance()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);
            var body = createBalanceDtoBuilder.WithAccountId(account.Id).Generate();

            await client.PostAsync(baseEndpoint, body);

            var balance = this.GetBalance(body);
            Assert.NotNull(balance);
        }
        
        [Test]
        public async Task Post_ValidBalance_CreatesBalanceWithAmountZero()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);
            var body = createBalanceDtoBuilder.WithAccountId(account.Id).Generate();

            var response = await client.PostAsync(baseEndpoint, body);
            var result = await response.ReadContentAsync<SaveResponse<BalanceDto>>();
            Assert.Zero(result!.Data.Amount);
        }
    }
}
