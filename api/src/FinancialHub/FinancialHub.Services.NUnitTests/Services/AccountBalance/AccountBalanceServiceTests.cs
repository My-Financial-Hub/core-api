using System;
using Moq;
using NUnit.Framework;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Services.Services;
using FinancialHub.Domain.Tests.Builders.Models;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class AccountBalanceServiceTests
    {
        protected Random random;
        protected AccountModelBuilder accountModelBuilder;
        protected BalanceModelBuilder balanceModelBuilder;

        private IAccountBalanceService service;

        private Mock<IBalancesService> balanceService;
        private Mock<IAccountsService> accountsService;

        [SetUp]
        public void Setup()
        {
            this.balanceService = new Mock<IBalancesService>();
            this.accountsService = new Mock<IAccountsService>();
            this.service = new AccountBalanceService(
                balanceService.Object,
                accountsService.Object
            );

            this.random = new Random();

            this.accountModelBuilder = new AccountModelBuilder();
            this.balanceModelBuilder = new BalanceModelBuilder();
        }
    }
}
