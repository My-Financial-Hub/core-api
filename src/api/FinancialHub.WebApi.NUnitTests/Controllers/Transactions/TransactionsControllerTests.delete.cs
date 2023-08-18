namespace FinancialHub.Core.WebApi.NUnitTests.Controllers
{
    public partial class TransactionsControllerTests
    {
        [Test]
        [TestCase(Description = "Delete Transaction Returns NoContent", Category = "Delete")]
        public async Task DeleteMyTransactions_ServiceSuccess_ReturnsNoContent()
        {
            var mock = this.transactionModelBuilder.Generate();
            var response = await this.controller.DeleteTransaction(mock.Id.GetValueOrDefault());

            var result = response as ObjectResult;

            Assert.IsNull(result?.Value);
        }
    }
}
