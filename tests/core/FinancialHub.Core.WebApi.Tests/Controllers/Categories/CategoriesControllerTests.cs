using FinancialHub.Core.WebApi.Controllers;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Categories;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class CategoriesControllerTests
    {
        private Random random;
        private CategoryDtoBuilder categoryDtoBuilder;
        private CreateCategoryDtoBuilder createCategoryDtoBuilder;
        private UpdateCategoryDtoBuilder updateCategoryDtoBuilder;

        private CategoriesController controller;
        private Mock<ICategoriesService> mockService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();

            this.categoryDtoBuilder = new CategoryDtoBuilder();
            this.createCategoryDtoBuilder = new CreateCategoryDtoBuilder();
            this.updateCategoryDtoBuilder = new UpdateCategoryDtoBuilder();

            this.mockService = new Mock<ICategoriesService>();
            this.controller = new CategoriesController(this.mockService.Object);
        }
    }
}