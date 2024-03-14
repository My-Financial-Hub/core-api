using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Accounts;

namespace FinancialHub.Core.IntegrationTests.Controllers
{
    public partial class AccountsControllerTests
    {
        private UpdateAccountDtoBuilder updateAccountDtoBuilder;
        protected void AddUpdateAccountBuilder()
        {
            updateAccountDtoBuilder = new UpdateAccountDtoBuilder();
        }

        [Test]
        public async Task Put_InvalidAccount_Returns400BadRequest()
        {
            var id = Guid.NewGuid();
            fixture.AddData(accountBuilder.WithId(id).Generate());

            var body = updateAccountDtoBuilder
                .WithName(string.Empty)
                .WithDescription(new string('o', 501))
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Put_InvalidAccount_ReturnsAccountValidationError()
        {
            var id = Guid.NewGuid();
            fixture.AddData(accountBuilder.WithId(id).Generate());

            var body = updateAccountDtoBuilder
                .WithName(string.Empty)
                .WithDescription(new string('o', 501))
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            var validationResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

            Assert.IsNotNull(validationResponse);
            Assert.AreEqual(2, validationResponse!.Errors.Length);
        }

        [Test]
        public async Task Put_ExistingAccount_ReturnUpdatedAccount()
        {
            var id = Guid.NewGuid();
            fixture.AddData(accountBuilder.WithId(id).Generate());

            var body = updateAccountDtoBuilder.Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<AccountDto>>();
            var resultData = result?.Data;
            Assert.That(resultData, Is.Not.Null);
            Assert.AreEqual(resultData?.Id, id);
            Assert.AreEqual(resultData?.Name, body.Name);
            Assert.AreEqual(resultData?.Description, body.Description);
            Assert.AreEqual(resultData?.IsActive, body.IsActive);
        }

        [Test]
        public async Task Put_ExistingAccount_UpdatesAccount()
        {
            var id = Guid.NewGuid();
            fixture.AddData(accountBuilder.WithId(id).Generate());

            var body = updateAccountDtoBuilder.Generate();
            await client.PutAsync($"{baseEndpoint}/{id}", body);

            var account = this.fixture.GetData<AccountEntity>().FirstOrDefault(x => x.Id == id);
            Assert.That(account, Is.Not.Null);
            Assert.AreEqual(account?.Id, id);
            Assert.AreEqual(account?.Name, body.Name);
            Assert.AreEqual(account?.Description, body.Description);
            Assert.AreEqual(account?.IsActive, body.IsActive);
        }

        [Test]
        public async Task Put_NonExistingAccount_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = updateAccountDtoBuilder.Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found Account with id {id}", result!.Message);
        }
    }
}
