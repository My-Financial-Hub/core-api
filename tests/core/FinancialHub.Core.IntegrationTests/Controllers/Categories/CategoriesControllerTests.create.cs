using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Accounts;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Categories;

namespace FinancialHub.Core.IntegrationTests.Controllers.Categories
{
    public partial class CategoriesControllerTests
    {
        private CreateCategoryDtoBuilder createCategoryDtoBuilder;
        protected void AddCreateCategoryBuilder()
        {
            createCategoryDtoBuilder = new CreateCategoryDtoBuilder();
        }

        protected CategoryEntity GetBalance(CreateCategoryDto balance)
        {
            return fixture
                .GetData<CategoryEntity>()
                .First(
                    bal =>
                        bal.Name        == balance.Name &&
                        bal.Description == balance.Description &&
                        bal.IsActive    == balance.IsActive
                );
        }

        [Test]
        public async Task Post_InvalidCategory_Returns400BadRequest()
        {
            var data = createCategoryDtoBuilder
                .WithName(string.Empty)
                .WithDescription(new string('o', 501))
                .Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Post_InvalidCategory_ReturnsCategoryValidationError()
        {
            var data = createCategoryDtoBuilder
                .WithName(string.Empty)
                .WithDescription(new string('o', 501))
                .Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            var validationResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

            Assert.IsNotNull(validationResponse);
            Assert.AreEqual(2, validationResponse!.Errors.Length);
        }

        [Test]
        public async Task Post_ValidCategory_ReturnCreatedCategory()
        {
            var body = createCategoryDtoBuilder.Generate();

            var response = await client.PostAsync(baseEndpoint, body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<CategoryDto>>();
            Assert.IsNotNull(result?.Data);
            var resultData = result!.Data;
            Assert.AreEqual(body.Name, resultData.Name);
            Assert.AreEqual(body.Description, resultData.Description);
            Assert.AreEqual(body.IsActive, resultData.IsActive);
        }

        [Test]
        public async Task Post_ValidCategory_CreatesCategory()
        {
            var body = createCategoryDtoBuilder.Generate();

            await client.PostAsync(baseEndpoint, body);

            var balance = this.GetBalance(body);
            Assert.IsNotNull(balance);
        }
    }
}
