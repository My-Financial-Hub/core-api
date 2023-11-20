using AutoMapper;
using FinancialHub.Core.Domain.Interfaces.Mappers;
using FinancialHub.Core.Domain.Interfaces.Repositories;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.Entities;
using FinancialHub.Core.Application.Mappers;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Providers;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        protected Random random;
        protected BalanceEntityBuilder balanceEntityBuilder;
        protected BalanceModelBuilder balanceModelBuilder;

        private IBalancesService service;

        private Mock<IBalancesProvider> provider;
        private Mock<IAccountsProvider> accountsProvider;

        [SetUp]
        public void Setup()
        {
            this.provider         = new Mock<IBalancesProvider>();
            this.accountsProvider = new Mock<IAccountsProvider>();
            this.service            = new BalancesService(
                provider.Object, 
                accountsProvider.Object
            );

            this.random = new Random();

            this.balanceEntityBuilder = new BalanceEntityBuilder();
            this.balanceModelBuilder = new BalanceModelBuilder();
        }

        public ICollection<BalanceEntity> GenerateBalances()
        {
            return this.balanceEntityBuilder.Generate(random.Next(5, 10));
        }
    }
}
