using FinancialHub.Core.WebApi.Controllers;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Categories;
using Microsoft.Extensions.Logging;

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
        private Mock<ILogger<CategoriesController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();

            this.categoryDtoBuilder = new CategoryDtoBuilder();
            this.createCategoryDtoBuilder = new CreateCategoryDtoBuilder();
            this.updateCategoryDtoBuilder = new UpdateCategoryDtoBuilder();

            this.mockService = new Mock<ICategoriesService>();
            this.mockLogger = new Mock<ILogger<CategoriesController>>();
            this.controller = new CategoriesController(this.mockService.Object, this.mockLogger.Object);
        }
    }
}