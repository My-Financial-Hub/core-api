namespace FinancialHub.Core.WebApi.NUnitTests.Controllers
{
    public partial class AccountsControllerTests
    {
        [Test]
        [TestCase(Description = "Update account existing Returns Ok", Category = "Update")]
        public async Task UpdateAccount_Valid_ReturnsOk()
        {
            var body = this.accountModelBuilder.Generate();
            var guid = body.Id.GetValueOrDefault();
            var mockResult = new ServiceResult<AccountModel>(body);

            this.mockService
                .Setup(x => x.UpdateAsync(guid, body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.UpdateAccount(guid, body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<AccountModel>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<AccountModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.UpdateAsync(guid, body), Times.Once);
        }

        [Test]
        [TestCase(Description = "Update Account invalid account returns BadRequest", Category = "Update")]
        public async Task UpdateAccount_Invalid_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.accountModelBuilder.Generate();
            var guid = body.Id.GetValueOrDefault();

            var mockResult = new ServiceResult<AccountModel>(body, new InvalidDataError(errorMessage));

            this.mockService
                .Setup(x => x.UpdateAsync(guid,body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.UpdateAccount(guid,body);

            var result = response as ObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
            Assert.IsInstanceOf<ValidationErrorResponse>(result?.Value);

            var listResponse = result?.Value as ValidationErrorResponse;
            Assert.AreEqual(mockResult.Error!.Code, listResponse?.Code);
            Assert.AreEqual(mockResult.Error.Message, listResponse?.Message);

            this.mockService.Verify(x => x.UpdateAsync(guid, body), Times.Once);
        }
    }
}
