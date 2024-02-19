namespace FinancialHub.Core.IntegrationTests.Controllers.Categories
{
    public partial class CategoriesControllerTests : BaseControllerTests
    {
        private TransactionEntityBuilder transactionBuilder;
        private CategoryEntityBuilder categoryBuilder;

        public CategoriesControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/categories")
        {

        }

        public override void SetUp()
        {
            transactionBuilder = new TransactionEntityBuilder();
            categoryBuilder = new CategoryEntityBuilder();
            AddCreateCategoryBuilder();
            AddUpdateCategoryBuilder();
            base.SetUp();
        }
    }
}