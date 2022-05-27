using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Results.Errors;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class AccountBalanceServiceTests
    {
        [Test]
        public async Task CreateAsync_CreatesAccount()
        {
            var account = this.accountModelBuilder.Generate();

            this.accountsService
                .Setup(x => x.CreateAsync(account))
                .ReturnsAsync(account)
                .Verifiable();
            this.balanceService
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await 
                    Task.FromResult(
                        new ServiceResult<BalanceModel>(x)
                    )
                )
                .Verifiable();

            var result = await this.service.CreateAsync(account);

            this.accountsService.Verify(x => x.CreateAsync(account),Times.Once);
        }

        [Test]
        public async Task CreateAsync_CreatesDefaultBalance()
        {
            var account = this.accountModelBuilder.Generate();

            this.accountsService
                .Setup(x => x.CreateAsync(account))
                .ReturnsAsync(account)
                .Verifiable();
            this.balanceService
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await
                    Task.FromResult(
                        new ServiceResult<BalanceModel>(x)
                    )
                )
                .Verifiable();

            var result = await this.service.CreateAsync(account);
            this.balanceService.Verify(x => x.CreateAsync(It.IsAny<BalanceModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidAccount_ReturnsCreatedAccount()
        {
            var account = this.accountModelBuilder.Generate();

            this.accountsService
                .Setup(x => x.CreateAsync(account))
                .ReturnsAsync(account)
                .Verifiable();
            this.balanceService
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await
                    Task.FromResult(
                        new ServiceResult<BalanceModel>(x)
                    )
                )
                .Verifiable();

            var result = await this.service.CreateAsync(account);

            Assert.IsFalse(result.HasError);
            Assert.AreEqual(account.Name,result.Data.Name);
            Assert.AreEqual(account.Description,result.Data.Description);
            Assert.AreEqual(account.IsActive,result.Data.IsActive);
        }

        [Test]
        public async Task CreateAsync_ErrorOnCreateAccount_ReturnsError()
        {
            var account = this.accountModelBuilder.Generate();
            var error = new ServiceError(1,"Account Error");

            this.accountsService
                .Setup(x => x.CreateAsync(account))
                .ReturnsAsync(account)
                .Verifiable();
            this.balanceService
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .ReturnsAsync(error)
                .Verifiable();

            var result = await this.service.CreateAsync(account);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(error.Message, result.Error.Message);
        }

        [Test]
        public async Task CreateAsync_ErrorOnCreateDefaultBalance_ReturnsError()
        {
            var account = this.accountModelBuilder.Generate();
            var error = new ServiceError(1, "Balance Error");

            this.accountsService
                .Setup(x => x.CreateAsync(account))
                .ReturnsAsync(error)
                .Verifiable();
            this.balanceService
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await
                    Task.FromResult(
                        new ServiceResult<BalanceModel>(x)
                    )
                )
                .Verifiable();

            var result = await this.service.CreateAsync(account);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(error.Message, result.Error.Message);
        }
    }
}
