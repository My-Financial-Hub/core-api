using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Categories;

namespace FinancialHub.Core.IntegrationTests.Controllers.Categories
{
    public partial class CategoriesControllerTests
    {
        private UpdateCategoryDtoBuilder updateCategoryDtoBuilder;
        protected void AddUpdateCategoryBuilder()
        {
            updateCategoryDtoBuilder = new UpdateCategoryDtoBuilder();
        }

        protected CategoryEntity GetBalance(UpdateCategoryDto balance)
        {
            return fixture
                .GetData<CategoryEntity>()
                .First(
                    bal =>
                        bal.Name == balance.Name &&
                        bal.Description == balance.Description &&
                        bal.IsActive == balance.IsActive
                );
        }

        [Test]
        public async Task Put_InvalidCategory_Returns400BadRequest()
        {
            var id = Guid.NewGuid();
            fixture.AddData(categoryBuilder.WithId(id).Generate());

            var data = updateCategoryDtoBuilder
                .WithName(string.Empty)
                .WithDescription(new string('o', 501))
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Put_InvalidCategory_ReturnsCategoryValidationError()
        {
            var id = Guid.NewGuid();
            fixture.AddData(categoryBuilder.WithId(id).Generate());

            var data = updateCategoryDtoBuilder
                .WithName(string.Empty)
                .WithDescription(new string('o', 501))
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", data);
            var validationResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

            Assert.IsNotNull(validationResponse);
            Assert.AreEqual(2, validationResponse!.Errors.Length);
        }

        [Test]
        public async Task Put_ExistingCategory_ReturnUpdatedCategory()
        {
            var id = Guid.NewGuid();
            fixture.AddData(categoryBuilder.WithId(id).Generate());

            var body = updateCategoryDtoBuilder.Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<CategoryDto>>();
            Assert.IsNotNull(result?.Data);
            var resultData = result!.Data;
            Assert.AreEqual(body.Name, resultData.Name);
            Assert.AreEqual(body.Description, resultData.Description);
            Assert.AreEqual(body.IsActive, resultData.IsActive);
        }

        [Test]
        public async Task Put_ExistingCategory_UpdatesCategory()
        {
            var id = Guid.NewGuid();
            fixture.AddData(categoryBuilder.WithId(id).Generate());

            var body = updateCategoryDtoBuilder.Generate();
            await client.PutAsync($"{baseEndpoint}/{id}", body);

            var balance = this.GetBalance(body);
            Assert.IsNotNull(balance);
        }

        [Test]
        public async Task Put_NonExistingCategory_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = updateCategoryDtoBuilder.Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found Category with id {id}", result!.Message);
        }
    }
}
