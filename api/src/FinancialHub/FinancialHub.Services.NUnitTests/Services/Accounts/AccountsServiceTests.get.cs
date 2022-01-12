using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class AccountsServiceTests
    {
        //TODO: change mock when filter by user
        [Test]
        [TestCase(Description = "Get by user sucess return",Category = "Get")]
        public async Task GetByUsersAsync_ValidUser_ReturnsAccounts()
        {
            var entitiesMock = Enumerable.Repeat(this.entityGenerator.GenerateAccount(),random.Next(10,100));
            
            this.repository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(entitiesMock.ToArray())
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<ICollection<AccountModel>>(It.IsAny<ICollection<AccountEntity>>()))
                .Returns<ICollection<AccountEntity>>((ent) => this.mapper.Map<ICollection<AccountModel>>(ent))
                .Verifiable();

            var result = await this.service.GetAllByUserAsync(string.Empty);

            Assert.IsInstanceOf<ServiceResult<ICollection<AccountModel>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(entitiesMock.Count(), result.Data.Count());

            this.mapperWrapper.Verify(x => x.Map<ICollection<AccountModel>>(It.IsAny<ICollection<AccountEntity>>()),Times.Once);
            this.repository.Verify(x => x.GetAllAsync(),Times.Once());
        }


        [Test]
        [TestCase(Description = "Get by user repository exception", Category = "Get")]
        public async Task GetByUsersAsync_RepositoryException_ThrowsException()
        {
            var entitiesMock = Enumerable.Repeat(this.entityGenerator.GenerateAccount(), random.Next(10, 100));
            var exc = new Exception("mock");
            this.repository
                .Setup(x => x.GetAllAsync())
                .Throws(exc)
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<IEnumerable<AccountModel>>(It.IsAny<IEnumerable<AccountEntity>>()))
                .Returns<IEnumerable<AccountEntity>>((ent) => this.mapper.Map<IEnumerable<AccountModel>>(ent))
                .Verifiable();

            var exception = Assert.ThrowsAsync<Exception>(
                async ()=> await this.service.GetAllByUserAsync(string.Empty)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.GetAllAsync(), Times.Once());
        }
    }
}
