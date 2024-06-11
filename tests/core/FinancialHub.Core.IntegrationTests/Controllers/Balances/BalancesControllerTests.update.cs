using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances;

namespace FinancialHub.Core.IntegrationTests.Controllers
{
    public partial class BalancesControllerTests
    {
        private UpdateBalanceDtoBuilder updateBalanceDtoBuilder;
        protected void AddUpdateBalanceBuilder()
        {
            updateBalanceDtoBuilder = new UpdateBalanceDtoBuilder();
        }

        protected BalanceEntity GetBalance(UpdateBalanceDto balance)
        {
            return fixture
                .GetData<BalanceEntity>()
                .First(
                    bal =>
                        bal.Name == balance.Name &&
                        bal.Currency == balance.Currency &&
                        bal.AccountId == balance.AccountId &&
                        bal.IsActive == balance.IsActive
                );
        }

        [Test]
        public async Task Put_InvalidBalance_Returns400BadRequest()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = balanceBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();
            fixture.AddData(entity);

            var body = updateBalanceDtoBuilder
                .WithName(string.Empty)
                .WithCurrency(new string('o', 51))
                .WithAccountId(account.Id)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Put_InvalidBalance_ReturnsBalanceValidationError()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = balanceBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();
            fixture.AddData(entity);

            var body = updateBalanceDtoBuilder
                .WithName(string.Empty)
                .WithCurrency(new string('o', 51))
                .WithAccountId(account.Id)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            var validationResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

            Assert.IsNotNull(validationResponse);
            Assert.AreEqual(2, validationResponse!.Errors.Length);
        }

        [Test]
        public async Task Put_ExistingBalance_ReturnsUpdatedBalance()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = balanceBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();
            fixture.AddData(entity);

            var body = updateBalanceDtoBuilder
                .WithAccountId(account.Id)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceDto>>();
            var resultData = result?.Data;
            Assert.IsNotNull(resultData);
            Assert.AreEqual(body.Name, resultData?.Name);
            Assert.AreEqual(body.Currency, resultData?.Currency);
            Assert.AreEqual(body.IsActive, resultData?.IsActive);
        }

        [Test]
        public async Task Put_ExistingBalance_DoesNotUpdateBalanceAmount()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = balanceBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();
            fixture.AddData(entity);

            var body = updateBalanceDtoBuilder
                .WithAccountId(account.Id)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceDto>>();
            var resultData = result?.Data;
            Assert.IsNotNull(resultData);
            Assert.AreEqual(entity.Amount, resultData?.Amount);
        }

        [Test]
        public async Task Put_ExistingBalance_UpdatesBalance()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var id = Guid.NewGuid();
            var balance = balanceBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();
            fixture.AddData(balance);

            var newBalance = updateBalanceDtoBuilder
                .WithAccountId(account.Id)
                .Generate();

            await client.PutAsync($"{baseEndpoint}/{id}", newBalance);

            var result = this.GetBalance(newBalance);
            Assert.AreEqual(id, result?.Id);
            Assert.AreEqual(newBalance.Name, result?.Name);
            Assert.AreEqual(newBalance.Currency, result?.Currency);
            Assert.AreEqual(newBalance.IsActive, result?.IsActive);
        }

        [Test]
        public async Task Put_ExistingBalance_DoesNotUpdatesBalanceAmount()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = balanceBuilder
                .WithAccountId(account.Id)
                .WithAmount(0)
                .WithId(id)
                .Generate();
            fixture.AddData(entity);

            var data = updateBalanceDtoBuilder
                .WithAccountId(account.Id)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceDto>>();
            Assert.Zero(result!.Data.Amount);
        }

        [Test]
        public async Task Put_NotExistingBalance_ReturnsNotFound()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var entity = balanceBuilder
                .WithAccountId(account.Id)
                .Generate();
            fixture.AddData(entity);

            var id = Guid.NewGuid();
            var data = updateBalanceDtoBuilder
                .WithAccountId(account.Id)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual(result?.Message, $"Not found Balance with id {id}");
        }
    }
}
