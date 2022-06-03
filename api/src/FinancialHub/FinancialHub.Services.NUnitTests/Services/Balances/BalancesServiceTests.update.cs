using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task UpdateAsync_ValidatesIfAccountExists()
        {
            var model = this.balanceModelBuilder.Generate();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            this.accountsRepository.Verify(x => x.GetByIdAsync(model.AccountId), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidatesIfBalanceExists()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(default(BalanceEntity))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            this.repository.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_UpdatesBalance()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            this.repository.Verify(x => x.UpdateAsync(It.IsAny<BalanceEntity>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidBalanceModel_ReturnsBalanceModel()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task UpdateAsync_NonExistingBalanceId_ReturnsResultError()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(default(BalanceEntity))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Balance with id {model.Id}", result.Error.Message);

            this.repository.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<BalanceEntity>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_NonExistingAccountId_ReturnsResultError()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Account with id {model.AccountId}", result.Error.Message);
        }
    }
}
