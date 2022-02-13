using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class AccountsControllerTests
    {
        [Test]
        public async Task DeleteMyAccounts_ServiceSuccess_ReturnsNoContent()
        {
            var mock = this.modelGenerator.GenerateAccount();
            var response = await this.controller.DeleteAccount(mock.Id.GetValueOrDefault());

            var result = response as ObjectResult;

            Assert.AreEqual(204, result?.StatusCode);
            Assert.IsNull(result?.Value);
        }
    }
}
