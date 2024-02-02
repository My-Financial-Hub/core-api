using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class BalancesControllerTests
    {
        [Test]
        public async Task UpdateBalance_Valid_ReturnsOk()
        {
            var body = this.updateBalanceDtoBuilder.Generate();
            var balanceResult = this.balanceDtoBuilder
                .FromUpdateDto(body)
                .Generate();
            var guid = balanceResult.Id;
            var mockResult = new ServiceResult<BalanceDto>(balanceResult);

            this.mockService
                .Setup(x => x.UpdateAsync(guid, body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.UpdateBalance(guid, body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<BalanceDto>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<BalanceDto>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.UpdateAsync(guid, body), Times.Once);
        }

        [Test]
        public async Task UpdateBalance_Invalid_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.updateBalanceDtoBuilder.Generate();
            var balanceResult = this.balanceDtoBuilder
                .FromUpdateDto(body)
                .Generate();
            var guid = balanceResult.Id;
            var mockResult = new ServiceResult<BalanceDto>(balanceResult, new InvalidDataError(errorMessage));

            this.mockService
                .Setup(x => x.UpdateAsync(guid,body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.UpdateBalance(guid,body);

            var result = response as ObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
            Assert.IsInstanceOf<ValidationErrorResponse>(result?.Value);

            var listResponse = result?.Value as ValidationErrorResponse;
            Assert.AreEqual(mockResult.Error!.Code, listResponse?.Code);
            Assert.AreEqual(mockResult.Error!.Message, listResponse?.Message);

            this.mockService.Verify(x => x.UpdateAsync(guid, body), Times.Once);
        }
    }
}
