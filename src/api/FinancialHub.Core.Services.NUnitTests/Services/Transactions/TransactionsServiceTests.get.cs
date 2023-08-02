using System.Linq;
using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Queries;

namespace FinancialHub.Core.Services.NUnitTests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task GetByUsersAsync_ValidUser_ReturnsTransactions()
        {
            var filter = new TransactionFilter();
            var entitiesMock = this.GenerateTransactions();

            this.repository
                .Setup(x => x.GetAsync(It.IsAny<Func<TransactionEntity,bool>>()))
                .ReturnsAsync(entitiesMock.ToArray());

            this.mapperWrapper
                .Setup(x => x.Map<TransactionQuery>(It.IsAny<TransactionFilter>()))
                .Returns<TransactionFilter>((ent) => this.mapper.Map<TransactionQuery>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<IEnumerable<TransactionModel>>(It.IsAny<IEnumerable<TransactionEntity>>()))
                .Returns<IEnumerable<TransactionEntity>>((ent) => this.mapper.Map<IEnumerable<TransactionModel>>(ent))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.GetAllByUserAsync(string.Empty, filter);

            Assert.IsInstanceOf<ServiceResult<ICollection<TransactionModel>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(entitiesMock.Count, result.Data.Count);
        }

        [Test]  
        public async Task GetByIdAsync_ExistingTransaction_ReturnsTransaction()
        {
            var id = Guid.NewGuid();
            var transaction = this.transactionBuilder
                .WithId(id)
                .Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(transaction);

            this.SetUpMapper();

            var result = await this.service.GetByIdAsync(id);

            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);
            Assert.AreEqual(transaction.Id, result.Data.Id);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingTransaction_ReturnsNotFoundError()
        {
            var id = Guid.NewGuid();

            this.SetUpMapper();

            var result = await this.service.GetByIdAsync(id);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Transaction with id {id}", result.Error.Message);
        }
    }
}
