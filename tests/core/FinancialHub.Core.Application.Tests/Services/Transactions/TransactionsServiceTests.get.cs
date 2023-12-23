using FinancialHub.Core.Domain.Filters;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task GetByUsersAsync_ValidUser_ReturnsTransactions()
        {
            var filter = new TransactionFilter();
            var entitiesMock = this.transactionModelBuilder
                .Generate(random.Next(10, 20));

            this.provider
                .Setup(x => x.GetAllAsync(filter))
                .ReturnsAsync(entitiesMock.ToArray());

            var result = await this.service.GetAllByUserAsync(string.Empty, filter);

            Assert.IsInstanceOf<ServiceResult<ICollection<TransactionModel>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(entitiesMock.Count, result.Data!.Count);
        }

        [Test]  
        public async Task GetByIdAsync_ExistingTransaction_ReturnsTransaction()
        {
            var id = Guid.NewGuid();
            var transaction = this.transactionModelBuilder
                .WithId(id)
                .Generate();

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(transaction);

            var result = await this.service.GetByIdAsync(id);

            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);
            Assert.AreEqual(transaction.Id, result.Data!.Id);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingTransaction_ReturnsNotFoundError()
        {
            var id = Guid.NewGuid();

            var result = await this.service.GetByIdAsync(id);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Transaction with id {id}", result.Error!.Message);
        }
    }
}
