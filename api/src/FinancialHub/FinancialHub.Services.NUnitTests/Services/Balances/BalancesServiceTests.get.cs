using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task GetByAccountAsync_ValidAccount_ReturnsBalances()
        {
            var firstEntity = this.balanceEntityBuilder.Generate();
            var entitiesMock = this.balanceEntityBuilder
                .WithAccount(firstEntity.Account)
                .Generate(random.Next(5, 10));

            this.repository
                .Setup(x => x.GetAsync(It.IsAny<Func<BalanceEntity,bool>>()))
                .ReturnsAsync(entitiesMock.ToArray())
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<ICollection<BalanceModel>>(It.IsAny<ICollection<BalanceEntity>>()))
                .Returns<ICollection<BalanceEntity>>((ent) => this.mapper.Map<ICollection<BalanceModel>>(ent))
                .Verifiable();

            var result = await this.service.GetAllByAccountAsync(firstEntity.Account.Id.GetValueOrDefault());

            Assert.IsInstanceOf<ServiceResult<ICollection<BalanceModel>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(entitiesMock.Count, result.Data.Count);

            this.repository.Verify(x => x.GetAsync(It.IsAny<Func<BalanceEntity, bool>>()), Times.Once);
        }

        [Test]
        public void GetByAccountAsync_RepositoryException_ThrowsException()
        {
            var entitiesMock = this.GenerateBalances();

            var exc = new Exception("mock");
            this.repository
                .Setup(x => x.GetAsync(It.IsAny<Func<BalanceEntity, bool>>()))
                .Throws(exc)
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<IEnumerable<AccountModel>>(It.IsAny<IEnumerable<AccountEntity>>()))
                .Returns<IEnumerable<AccountEntity>>((ent) => this.mapper.Map<IEnumerable<AccountModel>>(ent))
                .Verifiable();

            var exception = Assert.ThrowsAsync<Exception>(
                async () => await this.service.GetAllByAccountAsync(Guid.Empty)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.GetAsync(It.IsAny<Func<BalanceEntity, bool>>()), Times.Once);
        }
    }
}
