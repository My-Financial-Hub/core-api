namespace FinancialHub.Core.IntegrationTests
{
    public class CategoriesControllerTests : BaseControllerTests
    {
        private CategoryEntityBuilder dataBuilder;
        private CategoryModelBuilder builder;

        public CategoriesControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/categories")
        {

        }

        public override void SetUp()
        {
            this.dataBuilder = new CategoryEntityBuilder();
            this.builder = new CategoryModelBuilder(); 
            base.SetUp();
        }

        protected async Task AssertGetExists(CategoryModel expected)
        {
            var getResponse = await this.client.GetAsync(baseEndpoint);

            var getResult = await getResponse.ReadContentAsync<ListResponse<CategoryModel>>();
            Assert.AreEqual(1, getResult?.Data.Count);
            CategoryModelAssert.Equal(expected, getResult!.Data.First());
        }

        [Test]
        public async Task GetAll_ReturnCategories()
        {
            var data = dataBuilder.Generate(10);
            this.fixture.AddData(data.ToArray());

            var response = await this.client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<CategoryModel>>();
            Assert.AreEqual(data.Count, result?.Data.Count);
        }

        [Test]
        public async Task Post_ValidCategory_ReturnCreatedCategory()
        {
            var data = this.builder.Generate();

            var response = await this.client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<CategoryModel>>();
            Assert.IsNotNull(result?.Data);
            CategoryModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidCategory_CreateCategory()
        {
            var body = this.builder.Generate();

            await this.client.PostAsync(baseEndpoint, body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_ExistingCategory_ReturnUpdatedCategory()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(dataBuilder.WithId(id).Generate());

            var body = this.builder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<CategoryModel>>();
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            CategoryModelAssert.Equal(body,result!.Data);
        }

        [Test]
        public async Task Put_ExistingCategory_UpdatesCategory()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(dataBuilder.WithId(id).Generate());

            var body = this.builder.WithId(id).Generate();
            await this.client.PutAsync($"{baseEndpoint}/{id}", body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_NonExistingCategory_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = this.builder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found category with id {id}", result!.Message);
        }

        [Test]
        public async Task Delete_ReturnNoContent()
        {
            var id = Guid.NewGuid();

            var data = dataBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            var response = await this.client.DeleteAsync($"{baseEndpoint}/{id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_RemovesCategoryFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = dataBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            await this.client.DeleteAsync($"{baseEndpoint}/{id}");

            var getResponse = await this.client.GetAsync(baseEndpoint);
            var getResult = await getResponse.ReadContentAsync<ListResponse<CategoryModel>>();
            Assert.IsEmpty(getResult!.Data);
        }
    }
}