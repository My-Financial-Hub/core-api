namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task CreateAsync_ValidTransaction_CreatesTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesProvider
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(model.Category);

            this.balancesProvider
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(model.Balance);

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<TransactionModel>()))
                .Returns<TransactionModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            await this.service.CreateAsync(model);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<TransactionModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidTransaction_ReturnsCreatedTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesProvider
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(model.Category);

            this.balancesProvider
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(model.Balance);

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<TransactionModel>()))
                .Returns<TransactionModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);
        }

        [Test]
        public async Task CreateAsync_InvalidCategory_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.balancesProvider
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(model.Balance);

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Category with id {model.CategoryId}", result.Error!.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidBalance_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesProvider
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(model.Category);

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Balance with id {model.BalanceId}", result.Error!.Message);
        }
    }
}
