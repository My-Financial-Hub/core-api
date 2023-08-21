namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class TransactionsControllerTests
    {
        [Test]
        [TestCase(Description = "Create valid transaction Return Ok", Category = "Create")]
        public async Task CreateTransaction_Valid_ReturnsOk()
        {
            var body = this.transactionModelBuilder.Generate();
            var mockResult = new ServiceResult<TransactionModel>(body);

            this.mockTransactionBalanceServiceService
                .Setup(x => x.CreateTransactionAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateTransaction(body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<TransactionModel>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<TransactionModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockTransactionBalanceServiceService.Verify(x => x.CreateTransactionAsync(body), Times.Once);
        }

        [Test]
        [TestCase(Description = "Create invalid Transaction Return BadRequest", Category = "Create")]
        public async Task CreateTransaction_Invalid_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.transactionModelBuilder.Generate();

            var mockResult = new ServiceResult<TransactionModel>(body, new InvalidDataError(errorMessage));

            this.mockTransactionBalanceServiceService
                .Setup(x => x.CreateTransactionAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateTransaction(body);

            var result = response as ObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
            Assert.IsInstanceOf<ValidationErrorResponse>(result?.Value);

            var listResponse = result?.Value as ValidationErrorResponse;
            Assert.AreEqual(mockResult.Error.Code, listResponse?.Code);
            Assert.AreEqual(mockResult.Error.Message, listResponse?.Message);

            this.mockTransactionBalanceServiceService.Verify(x => x.CreateTransactionAsync(body), Times.Once);
        }
    }
}
