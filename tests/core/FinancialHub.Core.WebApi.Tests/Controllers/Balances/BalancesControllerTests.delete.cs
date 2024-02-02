namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class BalancesControllerTests
    {
        [Test]
        public async Task DeleteMyBalances_ServiceSuccess_ReturnsNoContent()
        {
            var mock = this.balanceDtoBuilder.Generate();
            var response = await this.controller.DeleteBalance(mock.Id.GetValueOrDefault());

            var result = response as ObjectResult;

            Assert.IsNull(result?.Value);
        }
    }
}
