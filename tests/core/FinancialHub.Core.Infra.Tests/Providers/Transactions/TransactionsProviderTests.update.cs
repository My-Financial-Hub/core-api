namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class TransactionsProviderTests
    {
        [Test]
        public async Task UpdateAsync_ReturnsUpdatedTransaction()
        {
            var id = Guid.NewGuid();
            var transaction = this.transactionModelBuilder
                .WithId(id)
                .Generate();

            repository
                .Setup(x => x.UpdateAsync(It.Is<TransactionEntity>(x => x.Id == id)))
                .Returns<TransactionEntity>(async x => await Task.FromResult(x));

            var result = await provider.UpdateAsync(id, transaction);

            TransactionModelAssert.Equal(transaction, result);
        }

        [Test]
        public async Task UpdateAsync_CallsTransactionsRepository()
        {
            var id = Guid.NewGuid();
            var transaction = this.transactionModelBuilder
                .WithId(id)
                .Generate();

            await provider.UpdateAsync(id, transaction);

            repository.Verify(x => x.UpdateAsync(It.IsAny<TransactionEntity>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_CallsTransactionsRepositoryWithIDFromParam()
        {
            var id = Guid.NewGuid();
            var transaction = this.transactionModelBuilder
                .WithId(id)
                .Generate();

            await provider.UpdateAsync(id, transaction);
            repository.Verify(x => x.UpdateAsync(It.Is<TransactionEntity>(x => x.Id == id)), Times.Once);
        }
    }
}
