using AutoMapper;
using System;

namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class AccountsProviderTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsCategoryList()
        {
            var accountEntities = accountEntityBuilder.Generate(random.Next(0, 10));
            var expectedAccounts = mapper.Map<IEnumerable<AccountModel>>(accountEntities);

            repository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(accountEntities);

            var accounts = await provider.GetAllAsync();

            AccountModelAssert.Equal(expectedAccounts.ToArray(), accounts.ToArray());
        }

        [Test]
        public async Task GetByIdAsync_ExistingCategory_ReturnsCategory()
        {
            var id = Guid.NewGuid();
            var accountEntity = accountEntityBuilder
                .WithId(id)
                .Generate();
            var expectedAccount = mapper.Map<AccountModel>(accountEntity);

            repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(accountEntity);

            var result = await provider.GetByIdAsync(id);

            AccountModelAssert.Equal(expectedAccount, result);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingCategory_ReturnsNull()
        {
            var id = Guid.NewGuid();

            var result = await provider.GetByIdAsync(id);

            Assert.That(result, Is.Null);
        }
    }
}
