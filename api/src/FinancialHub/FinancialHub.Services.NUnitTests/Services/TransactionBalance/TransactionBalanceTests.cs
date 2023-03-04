using System;
using Moq;
using NUnit.Framework;
using FinancialHub.Services.Services;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Tests.Builders.Models;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionBalanceTests
    {
        protected Random random;

        protected Mock<IBalancesService> balancesService;
        protected Mock<ITransactionsService> transactionsService;

        protected BalanceModelBuilder balanceModelBuilder;
        protected TransactionModelBuilder transactionModelBuilder;

        protected ITransactionBalanceService service;

        [SetUp]
        public void Setup()
        {
            this.balancesService = new Mock<IBalancesService>();
            this.transactionsService = new Mock<ITransactionsService>();
            this.service = new TransactionBalanceService(
                transactionsService.Object,
                balancesService.Object
            );

            this.random = new Random();

            this.balanceModelBuilder = new BalanceModelBuilder();
            this.transactionModelBuilder = new TransactionModelBuilder();
        }
    }
}
