namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class TransactionsProviderTests
    {
        //TODO: get with filters

        [Test]
        public async Task GetByIdAsync_ExistingTransaction_ReturnsTransaction()
        {
            var id = Guid.NewGuid();
            var transactionEntity = transactionBuilder
                .WithId(id)
                .Generate();
            var expectedTransaction = mapper.Map<TransactionModel>(transactionEntity);

            repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(transactionEntity);

            var result = await provider.GetByIdAsync(id);

            TransactionModelAssert.Equal(expectedTransaction, result);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingTransaction_ReturnsNull()
        {
            var id = Guid.NewGuid();

            var result = await provider.GetByIdAsync(id);

            Assert.That(result, Is.Null);
        }
    }
}
