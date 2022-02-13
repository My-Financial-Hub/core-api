using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class CategoriesControllerTests
    {
        [Test]
        public async Task DeleteMyCategories_ServiceSuccess_ReturnsNoContent()
        {
            var mock = this.modelGenerator.GenerateAccount();
            var response = await this.controller.DeleteCategory(mock.Id.GetValueOrDefault());

            var result = response as ObjectResult;

            Assert.IsNull(result?.Value);
        }
    }
}
