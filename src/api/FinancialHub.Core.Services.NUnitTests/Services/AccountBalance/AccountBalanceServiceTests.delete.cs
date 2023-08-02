namespace FinancialHub.Core.Services.NUnitTests.Services
{
    public partial class AccountBalanceServiceTests
    {
        [Test]
        public async Task DeleteAsync_RemovesBalances()
        {
            var expectedResult = random.Next(1, 100);
            var account = this.accountModelBuilder.Generate();
            var balances = this.balanceModelBuilder
                .WithAccount(account)
                .Generate(expectedResult);

            this.accountsService
                .Setup(x => x.DeleteAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(0)
                .Verifiable();
            this.balanceService
                .Setup(x => x.GetAllByAccountAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(balances)
                .Verifiable();
            this.balanceService
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(1)
                .Verifiable();

            var result = await this.service.DeleteAsync(account.Id.GetValueOrDefault());

            Assert.AreEqual(expectedResult, result.Data);

            this.balanceService.Verify(x => x.GetAllByAccountAsync(account.Id.GetValueOrDefault()),Times.Once);
            this.balanceService.Verify(x => x.DeleteAsync(It.IsAny<Guid>()),Times.Exactly(expectedResult));
        }

        [Test]
        public async Task DeleteAsync_RemovesAccount()
        {
            var account = this.accountModelBuilder.Generate();
            var balances = this.balanceModelBuilder
                .WithAccount(account)
                .Generate(0);

            this.accountsService
                .Setup(x => x.DeleteAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(1)
                .Verifiable();

            this.balanceService
                .Setup(x => x.GetAllByAccountAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(balances)
                .Verifiable();

            var result = await this.service.DeleteAsync(account.Id.GetValueOrDefault());

            Assert.AreEqual(1, result.Data);

            this.accountsService.Verify(x => x.DeleteAsync(account.Id.GetValueOrDefault()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_AccountDeletionError_ReturnsError()
        {
            var expectedResult = new ServiceError(1,"Error");
            var account = this.accountModelBuilder.Generate();
            var balances = this.balanceModelBuilder
                .WithAccount(account)
                .Generate(0);

            this.accountsService
                .Setup(x => x.DeleteAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(expectedResult)
                .Verifiable();
            this.balanceService
                .Setup(x => x.GetAllByAccountAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(balances)
                .Verifiable();
            var result = await this.service.DeleteAsync(account.Id.GetValueOrDefault());
            
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedResult,result.Error);
            Assert.AreEqual(0, result.Data);
        }

        [Test]
        public async Task DeleteAsync_BalanceDeletionError_ReturnsError()
        {
            var expectedResult = new ServiceError(1, "Error");
            var account = this.accountModelBuilder.Generate();
            var balances = this.balanceModelBuilder
                .WithAccount(account)
                .Generate(1);

            this.accountsService
                .Setup(x => x.DeleteAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(1)
                .Verifiable();
            this.balanceService
                .Setup(x => x.GetAllByAccountAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(balances)
                .Verifiable();
            this.balanceService
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResult)
                .Verifiable();

            var result = await this.service.DeleteAsync(account.Id.GetValueOrDefault());

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedResult, result.Error);
            Assert.AreEqual(0, result.Data);
        }

        [Test]
        public async Task DeleteAsync_Success_ReturnsRemovedBalancesAndAccount()
        {
            var expectedResult = random.Next(1, 100);
            var account = this.accountModelBuilder.Generate();
            var balances = this.balanceModelBuilder
                .WithAccount(account)
                .Generate(expectedResult);

            this.accountsService
                .Setup(x => x.DeleteAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(1)
                .Verifiable();
            this.balanceService
                .Setup(x => x.GetAllByAccountAsync(account.Id.GetValueOrDefault()))
                .ReturnsAsync(balances)
                .Verifiable();
            this.balanceService
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(1)
                .Verifiable();

            var result = await this.service.DeleteAsync(account.Id.GetValueOrDefault());

            Assert.IsFalse(result.HasError);
            Assert.AreEqual(expectedResult + 1, result.Data);
        }
    }
}
