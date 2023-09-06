namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class AccountsControllerTests
    {
        [Test]
        [TestCase(Description = "Get Accounts returns ok", Category = "Get")]
        public async Task GetMyAccounts_ServiceSuccess_ReturnsOk()
        {
            var mockResult = new ServiceResult<ICollection<AccountModel>>(
                accountModelBuilder.Generate(random.Next(0, 10))
            );

            this.mockService
                .Setup(x => x.GetAllByUserAsync(It.IsAny<string>()))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.GetMyAccounts();
            var result = (ObjectResult)response;

            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<ListResponse<AccountModel>>(result.Value);

            var listResponse = result.Value as ListResponse<AccountModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.GetAllByUserAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
