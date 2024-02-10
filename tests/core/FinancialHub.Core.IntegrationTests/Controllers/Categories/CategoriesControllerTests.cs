namespace FinancialHub.Core.IntegrationTests.Controllers.Categories
{
    public partial class CategoriesControllerTests : BaseControllerTests
    {
        private TransactionEntityBuilder transactionBuilder;
        private CategoryEntityBuilder dataBuilder;
        private CategoryModelBuilder builder;

        public CategoriesControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/categories")
        {

        }

        public override void SetUp()
        {
            transactionBuilder = new TransactionEntityBuilder();
            dataBuilder = new CategoryEntityBuilder();
            builder = new CategoryModelBuilder();
            base.SetUp();
        }

        protected async Task AssertGetExists(CategoryModel expected)
        {
            var getResponse = await client.GetAsync(baseEndpoint);

            var getResult = await getResponse.ReadContentAsync<ListResponse<CategoryModel>>();
            Assert.AreEqual(1, getResult?.Data.Count);
            CategoryModelAssert.Equal(expected, getResult!.Data.First());
        }
    }
}