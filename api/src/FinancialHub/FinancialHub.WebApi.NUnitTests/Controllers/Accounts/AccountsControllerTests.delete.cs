using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class AccountsControllerTests
    {
        [Test]
        [TestCase(Description = "Delete Account returns NoContent", Category = "Delete")]
        public async Task DeleteMyAccounts_ServiceSuccess_ReturnsNoContent()
        {
            var mock = this.accountModelBuilder.Generate();
            var response = await this.controller.DeleteAccount(mock.Id.GetValueOrDefault());

            var result = response as ObjectResult;

            Assert.IsNull(result?.Value);
        }
    }
}
