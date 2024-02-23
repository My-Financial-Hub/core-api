using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class AccountsControllerTests
    {
        [Test]
        [TestCase(Description = "Create account returns ok", Category = "Create")]
        public async Task CreateAccount_ServiceSuccess_ReturnsOk()
        {
            var body = this.createAccountDtoBuilder.Generate();
            var dtoResult = this.accountDtoBuilder
                .FromCreateDto(body)
                .Generate();
            var mockResult = new ServiceResult<AccountDto>(dtoResult);

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateAccount(body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<AccountDto>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<AccountDto>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.CreateAsync(body), Times.Once);
        }

        [Test]
        [TestCase(Description = "Create account returns Bad Request", Category = "Create")]
        public async Task CreateAccount_ServiceError_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.createAccountDtoBuilder.Generate();
            var dtoResult = this.accountDtoBuilder
                .FromCreateDto(body)
                .Generate();

            var mockResult = new ServiceResult<AccountDto>(dtoResult, new InvalidDataError(errorMessage));

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateAccount(body);

            var result = response as ObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
            Assert.IsInstanceOf<ValidationErrorResponse>(result?.Value);

            var listResponse = result?.Value as ValidationErrorResponse;
            Assert.AreEqual(mockResult.Error!.Code, listResponse?.Code);
            Assert.AreEqual(mockResult.Error.Message, listResponse?.Message);

            this.mockService.Verify(x => x.CreateAsync(body), Times.Once);
        }
    }
}
