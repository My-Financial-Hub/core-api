using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class TransactionsControllerTests
    {
        [Test]
        public async Task DeleteMyTransactions_ServiceSuccess_ReturnsNoContent()
        {
            var mock = this.modelGenerator.GenerateAccount();
            var response = await this.controller.DeleteTransaction(mock.Id.GetValueOrDefault());

            var result = response as ObjectResult;

            Assert.IsNull(result?.Value);
        }
    }
}
