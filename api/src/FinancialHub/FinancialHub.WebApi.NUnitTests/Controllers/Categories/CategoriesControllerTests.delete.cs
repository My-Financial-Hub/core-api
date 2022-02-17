using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class CategoriesControllerTests
    {
        [Test]
        public async Task DeleteMyCategories_ServiceSuccess_ReturnsNoContent()
        {
            var mock = this.categoryModelBuilder.Generate();
            var response = await this.controller.DeleteCategory(mock.Id.GetValueOrDefault());

            var result = response as ObjectResult;

            Assert.IsNull(result?.Value);
        }
    }
}
