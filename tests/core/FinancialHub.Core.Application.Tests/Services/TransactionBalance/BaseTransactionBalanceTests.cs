﻿using FinancialHub.Core.Application.Services;

namespace FinancialHub.Core.Application.Tests.Services.TransactionBalance
{
    public abstract class BaseTransactionBalanceTests
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
