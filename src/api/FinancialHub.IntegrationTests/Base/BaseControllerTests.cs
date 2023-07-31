using System.Net.Http;
using NUnit.Framework;
using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using FinancialHub.Common.Models;
using FinancialHub.Common.Entities;
using FinancialHub.Common.Responses.Success;
using FinancialHub.Common.Responses.Errors;
using FinancialHub.Common.Tests.Builders.Models;
using FinancialHub.Common.Tests.Builders.Entities;
using FinancialHub.IntegrationTests.Setup;
using FinancialHub.IntegrationTests.Extensions;

namespace FinancialHub.IntegrationTests.Base
{
    [TestFixtureSource(typeof(FinancialHubApiFixture))]
    public abstract class BaseControllerTests
    {
        protected readonly FinancialHubApiFixture fixture;
        protected HttpClient client => fixture.Client;

        protected readonly string baseEndpoint;
        protected readonly Random random;

        public BaseControllerTests(FinancialHubApiFixture fixture,string endpoint)
        {
            this.fixture = fixture;
            this.baseEndpoint = endpoint;
            this.random = new Random();
        }

        [SetUp]
        public virtual void SetUp()
        {
            this.fixture.CreateDatabase();
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.fixture.ClearData();
        }
    }

    [Obsolete("Not used")]
    [TestFixtureSource(typeof(FinancialHubApiFixture))]
    public abstract class BaseControllerTests<T,Y>
        where T : BaseModel
        where Y : BaseEntity
    {
        protected readonly FinancialHubApiFixture fixture;
        protected HttpClient client => fixture.Client;

        protected readonly string baseEndpoint;
        protected readonly string name;

        protected BaseModelBuilder<T> modelBuilder;
        protected BaseEntityBuilder<Y> entityBuilder;

        public BaseControllerTests(FinancialHubApiFixture fixture, string name ,string endpoint)
        {
            this.fixture = fixture;
            this.name = name;
            this.baseEndpoint = endpoint;
        }

        [SetUp]
        public virtual void SetUp()
        {
            this.fixture.CreateDatabase();
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.fixture.ClearData();
        }

        protected virtual void AssertEqual(T expected, T result)
        {

        }

        protected async Task AssertGetExists(T expected)
        {
            var getResponse = await this.client.GetAsync(baseEndpoint);

            var getResult = await getResponse.ReadContentAsync<ListResponse<T>>();
            Assert.AreEqual(1, getResult?.Data.Count);
            AssertEqual(expected, getResult!.Data.First());
        }

        [Test]
        public async Task GetAll_ReturnData()
        {
            var data = entityBuilder.Generate(10);
            this.fixture.AddData(data.ToArray());

            var response = await this.client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<T>>();
            Assert.AreEqual(data.Count, result?.Data.Count);
        }

        [Test]
        public async Task Post_ValidData_ReturnCreatedData()
        {
            var data = this.modelBuilder.Generate();

            var response = await this.client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<T>>();
            Assert.IsNotNull(result?.Data);
            AssertEqual(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidData_CreateData()
        {
            var body = this.modelBuilder.Generate();

            await this.client.PostAsync(baseEndpoint, body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_ExistingData_ReturnUpdatedData()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = this.modelBuilder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<T>>();
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            AssertEqual(body, result!.Data);
        }

        [Test]
        public async Task Put_ExistingData_UpdatesData()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = this.modelBuilder.WithId(id).Generate();
            await this.client.PutAsync($"{baseEndpoint}/{id}", body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_NonExistingData_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = this.modelBuilder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found {name} with id {id}", result!.Message);
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
        public async Task Delete_RemovesDataFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            await this.client.DeleteAsync($"{baseEndpoint}/{id}");

            var getResponse = await this.client.GetAsync(baseEndpoint);
            var getResult = await getResponse.ReadContentAsync<ListResponse<T>>();
            Assert.IsEmpty(getResult!.Data);
        }
    }
}
