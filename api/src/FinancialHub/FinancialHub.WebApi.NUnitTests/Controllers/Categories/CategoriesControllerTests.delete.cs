using NUnit.Framework;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class CategoriesControllerTests
    {
        [Test]
        public async Task DeleteMyAccounts_ServiceSuccess_ReturnsNoContent()
        {
            var mock = this.modelGenerator.GenerateAccount();
            var result = await this.controller.DeleteCategory(mock.Id.GetValueOrDefault());
        }
    }
}
