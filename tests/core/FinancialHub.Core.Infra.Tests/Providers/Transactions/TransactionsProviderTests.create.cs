namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class TransactionsProviderTests
    {
        [Test]
        public async Task CreateAsync_ReturnsCreatedTransactions()
        {
            var transaction = this.transactionModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async x => await Task.FromResult(x));

            var result = await this.provider.CreateAsync(transaction);

            Assert.That(result, Is.Not.Null);
            TransactionModelAssert.Equal(transaction, result);
        }

        [Test]
        public async Task CreateAsync_CallsCategoryRepository()
        {
            var category = this.transactionModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async x => await Task.FromResult(x));

            await this.provider.CreateAsync(category);

            repository.Verify(x => x.CreateAsync(It.IsAny<TransactionEntity>()), Times.Once);
        }
    }
}
