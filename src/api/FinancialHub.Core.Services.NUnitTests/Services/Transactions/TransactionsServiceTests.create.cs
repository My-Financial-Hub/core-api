namespace FinancialHub.Core.Services.NUnitTests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task CreateAsync_ValidTransaction_CreatesTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.balancesRepository
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance));

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            await this.service.CreateAsync(model);

            this.repository.Verify(x => x.CreateAsync(It.IsAny<TransactionEntity>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidTransaction_ReturnsCreatedTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.balancesRepository
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance));

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);
        }

        [Test]
        public async Task CreateAsync_InvalidCategory_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.SetUpMapper();

            this.balancesRepository
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance));

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Category with id {model.CategoryId}", result.Error.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidBalance_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.SetUpMapper();

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Balance with id {model.BalanceId}", result.Error.Message);
        }
    }
}
