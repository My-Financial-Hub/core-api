using System.Net;
using NUnit.Framework;
using System.Threading.Tasks;
using FinancialHub.IntegrationTests.Base;
using FinancialHub.Domain.Responses.Success;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Tests.Builders.Entities;
using FinancialHub.Domain.Tests.Builders.Models;
using System;
using System.Linq;
using FinancialHub.Domain.Responses.Errors;

namespace FinancialHub.IntegrationTests.Accounts
{
    public class AccountsControllerTests : BaseControllerTests
    {
        private readonly AccountEntityBuilder dataBuilder;
        private readonly AccountModelBuilder builder;

        private const string baseEndpoint = "/accounts";

        public AccountsControllerTests() : base()
        {
            this.dataBuilder = new AccountEntityBuilder();
            this.builder = new AccountModelBuilder();
        }

        protected static void AssertEqual(AccountModel expected, AccountModel result)
        {
            Assert.AreEqual(expected.Name,          result.Name);
            Assert.AreEqual(expected.Description,   result.Description);
            Assert.AreEqual(expected.Currency,      result.Currency);
            Assert.AreEqual(expected.IsActive,      result.IsActive);
        }

        protected async Task AssertGetExists(AccountModel expected)// TODO: change to get by ID url
        {
            var getResponse = await this.client.GetAsync(baseEndpoint);
            var getResult = await ReadContentAsync<ListResponse<AccountModel>>(getResponse.Content);
            Assert.AreEqual(1, getResult?.Data.Count);
            AssertEqual(expected, getResult!.Data.First());
        }

        [Test]
        public async Task GetAll_ReturnAccounts()
        {
            var data = dataBuilder.Generate(10);
            this.fixture.AddData(data);

            var response = await this.client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await ReadContentAsync<ListResponse<AccountModel>>(response.Content);
            Assert.AreEqual(data.Count, result?.Data.Count);
        }

        [Test]
        public async Task Post_ValidAccount_ReturnCreatedAccount()
        {
            var data = this.builder.Generate();

            var response = await this.client.PostAsync(baseEndpoint, CreateContent(data));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await ReadContentAsync<SaveResponse<AccountModel>>(response.Content);
            Assert.IsNotNull(result?.Data);
            AssertEqual(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidAccount_CreateAccount()
        {
            var body = this.builder.Generate();

            await this.client.PostAsync(baseEndpoint, CreateContent(body));

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_ExistingAccount_ReturnUpdatedAccount()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(dataBuilder.WithId(id).Generate());

            var body = this.builder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", CreateContent(body));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await ReadContentAsync<SaveResponse<AccountModel>>(response.Content);
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            AssertEqual(body,result!.Data);
        }

        [Test]
        public async Task Put_ExistingAccount_UpdatesAccount()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(dataBuilder.WithId(id).Generate());

            var body = this.builder.WithId(id).Generate();
            await this.client.PutAsync($"{baseEndpoint}/{id}", CreateContent(body));

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_NonExistingAccount_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = this.builder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", CreateContent(body));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await ReadContentAsync<NotFoundErrorResponse>(response.Content);
            Assert.AreEqual($"Not found account with id {id}", result!.Message);
        }
    }
}