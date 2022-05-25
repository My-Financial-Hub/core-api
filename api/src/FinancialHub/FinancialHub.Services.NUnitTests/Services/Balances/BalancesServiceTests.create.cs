using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task CreateAsync_ValidatesIfAccountExists()
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
        public async Task CreateAsync_CreatesBalance()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            this.repository.Verify(x => x.CreateAsync(It.IsAny<BalanceEntity>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidBalanceModel_ReturnsBalanceModel()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
        }

        [Test]
        public async Task CreateAsync_InvalidAccountModel_ReturnsNotFoundError()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Account with id {model.AccountId}", result.Error.Message);

            this.repository.Verify(x => x.CreateAsync(It.IsAny<BalanceEntity>()), Times.Never);
        }

        [Test]
        public void CreateAsync_RepositoryException_ThrowsException()
        {
            var model = this.balanceModelBuilder.Generate();
            var exc = new Exception("mock");

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<BalanceEntity>()))
                .Throws(exc)
                .Verifiable();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account));

            this.SetUpMapper();

            var exception = Assert.ThrowsAsync<Exception>(
                async () => await this.service.CreateAsync(model)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.CreateAsync(It.IsAny<BalanceEntity>()), Times.Once);
        }
    }
}
