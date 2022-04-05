using System;
using System.Net;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using FinancialHub.IntegrationTests.Base;
using FinancialHub.IntegrationTests.Setup;
using FinancialHub.IntegrationTests.Extensions;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Responses.Errors;
using FinancialHub.Domain.Responses.Success;
using FinancialHub.Domain.Tests.Builders.Models;
using FinancialHub.Domain.Tests.Builders.Entities;

namespace FinancialHub.IntegrationTests
{
    public class TransactionsControllerTests : BaseControllerTests
    {
        private TransactionEntityBuilder entityBuilder;
        private TransactionModelBuilder modelBuilder;

        private CategoryEntityBuilder categoryBuilder;
        private AccountEntityBuilder accountBuilder;

        public TransactionsControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/Transactions")
        {

        }

        public override void SetUp()
        {
            this.categoryBuilder = new CategoryEntityBuilder();
            this.accountBuilder = new AccountEntityBuilder();

            this.entityBuilder = new TransactionEntityBuilder();
            this.modelBuilder = new TransactionModelBuilder(); 
            base.SetUp();
        }

        protected static void AssertEqual(TransactionModel expected, TransactionModel result)
        {
            Assert.AreEqual(expected.Description,   result.Description);
            Assert.AreEqual(expected.IsActive,      result.IsActive);
        }

        protected async Task AssertGetExists(TransactionModel expected)
        {
            var getResponse = await this.client.GetAsync(baseEndpoint);

            var getResult = await getResponse.ReadContentAsync<ListResponse<TransactionModel>>();
            Assert.AreEqual(1, getResult?.Data.Count);
            AssertEqual(expected, getResult!.Data.First());
        }

        [Test]
        public async Task GetAll_ReturnTransactions()
        {
            var data = entityBuilder
                .Generate(10);
            this.fixture.AddData(data.ToArray());

            var response = await this.client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<TransactionModel>>();
            Assert.AreEqual(data.Count, result?.Data.Count);
        }

        [Test]
        public async Task Post_ValidTransaction_ReturnCreatedTransaction()
        {
            var data = this.modelBuilder.Generate();

            var response = await this.client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<TransactionModel>>();
            Assert.IsNotNull(result?.Data);
            AssertEqual(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidTransaction_CreateTransaction()
        {
            var body = this.modelBuilder.Generate();

            await this.client.PostAsync(baseEndpoint, body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_ExistingTransaction_ReturnUpdatedTransaction()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = this.modelBuilder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<TransactionModel>>();
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            AssertEqual(body,result!.Data);
        }

        [Test]
        public async Task Put_ExistingTransaction_UpdatesTransaction()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = this.modelBuilder.WithId(id).Generate();
            await this.client.PutAsync($"{baseEndpoint}/{id}", body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_NonExistingTransaction_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = this.modelBuilder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found Transaction with id {id}", result!.Message);
        }

        [Test]
        public async Task Delete_ReturnNoContent()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            var response = await this.client.DeleteAsync($"{baseEndpoint}/{id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_RemovesTransactionFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            await this.client.DeleteAsync($"{baseEndpoint}/{id}");

            var getResponse = await this.client.GetAsync(baseEndpoint);
            var getResult = await getResponse.ReadContentAsync<ListResponse<TransactionModel>>();
            Assert.IsEmpty(getResult!.Data);
        }
    }
}