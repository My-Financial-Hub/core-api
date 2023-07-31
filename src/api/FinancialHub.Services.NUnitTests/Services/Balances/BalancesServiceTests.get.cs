using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FinancialHub.Common.Results;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;

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
        public async Task GetByIdAsync_ValidId_ReturnsBalance()
        {
            var entity = this.balanceEntityBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(entity.Id.GetValueOrDefault()))
                .ReturnsAsync(entity)
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.GetByIdAsync(entity.Id.GetValueOrDefault());

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(entity.AccountId    , result.Data.AccountId);
            Assert.AreEqual(entity.Amount       , result.Data.Amount);
            Assert.AreEqual(entity.Name         , result.Data.Name);
            Assert.AreEqual(entity.IsActive     , result.Data.IsActive);

            this.repository.Verify(x => x.GetByIdAsync(entity.Id.GetValueOrDefault()), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNotFoundError()
        {
            var entity = this.balanceEntityBuilder.Generate();

            this.SetUpMapper();

            var result = await this.service.GetByIdAsync(entity.Id.GetValueOrDefault());

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsTrue(result.HasError);
        }
    }
}
