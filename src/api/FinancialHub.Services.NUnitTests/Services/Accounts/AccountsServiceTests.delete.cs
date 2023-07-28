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
        public async Task DeleteAsync_RepositorySuccess_ReturnsAccountModel()
        {
            var expectedResult = random.Next(1,100);
            var guid = Guid.NewGuid();
            this.repository
                .Setup(x => x.DeleteAsync(guid))
                .Returns(async () => await Task.FromResult(expectedResult))
                .Verifiable();

            var result = await this.service.DeleteAsync(guid);

            Assert.IsInstanceOf<ServiceResult<int>>(result);
            Assert.AreEqual(expectedResult,result.Data);
            this.repository.Verify(x => x.DeleteAsync(guid), Times.Once);
        }

        [Test]
        public void DeleteAsync_RepositoryException_ThrowsException()
        {
            var guid = Guid.NewGuid();
            var exc = new Exception("mock");

            this.repository
                .Setup(x => x.DeleteAsync(guid))
                .Throws(exc)
                .Verifiable();

            var exception = Assert.ThrowsAsync<Exception>(
                async () => await this.service.DeleteAsync(guid)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.DeleteAsync(guid), Times.Once());
        }
    }
}
