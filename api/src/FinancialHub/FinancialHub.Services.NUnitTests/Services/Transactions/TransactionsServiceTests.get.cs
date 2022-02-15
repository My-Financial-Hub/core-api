using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Filters;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Queries;
using FinancialHub.Domain.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionsServiceTests
    {
        //TODO: change mock when filter by user
        //TODO: add validation tests
        //TODO: add filter validation tests
        [Test]
        [TestCase(Description = "Get by user sucess return",Category = "Get")]
        public async Task GetByUsersAsync_ValidUser_ReturnsTransactions()
        {
            var filter = new TransactionFilter();
            var entitiesMock = this.GenerateTransactions();

            this.repository
                .Setup(x => x.GetAsync(It.IsAny<Func<TransactionEntity,bool>>()))
                .ReturnsAsync(entitiesMock.ToArray())
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<TransactionQuery>(It.IsAny<TransactionFilter>()))
                .Returns<TransactionFilter>((ent) => this.mapper.Map<TransactionQuery>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<IEnumerable<TransactionModel>>(It.IsAny<IEnumerable<TransactionEntity>>()))
                .Returns<IEnumerable<TransactionEntity>>((ent) => this.mapper.Map<IEnumerable<TransactionModel>>(ent))
                .Verifiable();

            var result = await this.service.GetAllByUserAsync(string.Empty, filter);

            Assert.IsInstanceOf<ServiceResult<ICollection<TransactionModel>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(entitiesMock.Count(), result.Data.Count);

            this.mapperWrapper.Verify(x => x.Map<TransactionQuery>(It.IsAny<TransactionFilter>()),Times.Once);
            this.mapperWrapper.Verify(x => x.Map<IEnumerable<TransactionModel>>(It.IsAny<IEnumerable<TransactionEntity>>()),Times.Once);
            this.repository.Verify(x => x.GetAsync(It.IsAny<Func<TransactionEntity, bool>>()), Times.Once());
        }


        [Test]
        [TestCase(Description = "Get by user repository exception", Category = "Get")]
        public async Task GetByUsersAsync_RepositoryException_ThrowsException()
        {
            var filter = new TransactionFilter();

            var entitiesMock = this.GenerateTransactions();
            var exc = new Exception("mock");

            this.mapperWrapper
                .Setup(x => x.Map<TransactionQuery>(It.IsAny<TransactionFilter>()))
                .Returns<TransactionFilter>((ent) => this.mapper.Map<TransactionQuery>(ent))
                .Verifiable();

            this.repository
                .Setup(x => x.GetAsync(It.IsAny<Func<TransactionEntity, bool>>()))
                .Throws(exc)
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<IEnumerable<TransactionModel>>(It.IsAny<IEnumerable<TransactionEntity>>()))
                .Returns<IEnumerable<TransactionEntity>>((ent) => this.mapper.Map<IEnumerable<TransactionModel>>(ent))
                .Verifiable();

            var exception = Assert.ThrowsAsync<Exception>(
                async ()=> await this.service.GetAllByUserAsync(string.Empty, filter)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.mapperWrapper.Verify(x => x.Map<TransactionQuery>(It.IsAny<TransactionFilter>()),Times.Once);
            this.repository.Verify(x => x.GetAsync(It.IsAny<Func<TransactionEntity, bool>>()), Times.Once());
        }
    }
}
