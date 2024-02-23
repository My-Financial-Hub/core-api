namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class BalancesControllerTests
    {
        [Test]
        public async Task DeleteMyBalances_ServiceSuccess_ReturnsNoContent()
        {
            var response = await this.controller.DeleteBalance(Guid.NewGuid());

            var result = response as ObjectResult;

            Assert.IsNull(result?.Value);
        }
    }
}
