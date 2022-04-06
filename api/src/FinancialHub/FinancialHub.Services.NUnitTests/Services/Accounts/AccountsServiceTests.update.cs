using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class AccountsServiceTests
    {
        [Test]
        [TestCase(Description = "Update valid account", Category = "Update")]
        public async Task UpdateAsync_ValidAccountModel_ReturnsAccountModel()
        {
            var model = this.accountModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<AccountModel>(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>((ent) => this.mapper.Map<AccountModel>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<AccountEntity>(It.IsAny<AccountModel>()))
                .Returns<AccountModel>((model) => this.mapper.Map<AccountEntity>(model))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<AccountModel>>(result);

            this.repository.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<AccountEntity>()), Times.Once);

            this.mapperWrapper.Verify(x => x.Map<AccountModel>(It.IsAny<AccountEntity>()), Times.Once);
            this.mapperWrapper.Verify(x => x.Map<AccountEntity>(It.IsAny<AccountModel>()), Times.Once);
        }

        [Test]
        [TestCase(Description = "Update non existing account", Category = "Update")]
        public async Task UpdateAsync_NonExistingAccountId_ReturnsResultError()
        {
            var model = this.accountModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(default(AccountEntity))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsInstanceOf<ServiceResult<AccountModel>>(result);
            Assert.IsTrue(result.HasError);

            this.repository.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<AccountEntity>()), Times.Never);
        }

        [Test]
        [TestCase(Description = "Update repository exception", Category = "Update")]
        public void UpdateAsync_RepositoryException_ThrowsException()
        {
            var model = this.accountModelBuilder.Generate();
            var exc = new Exception("mock");

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<AccountEntity>()))
                .Throws(exc)
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<AccountModel>(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>((ent) => this.mapper.Map<AccountModel>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<AccountEntity>(It.IsAny<AccountModel>()))
                .Returns<AccountModel>((model) => this.mapper.Map<AccountEntity>(model))
                .Verifiable();

            var exception = Assert.ThrowsAsync<Exception>(
                async () => await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<AccountEntity>()), Times.Once());
        }
    }
}
