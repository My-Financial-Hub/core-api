using Moq;
using System;
using NUnit.Framework;
using FinancialHub.WebApi.Controllers;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.NUnitTests.Generators;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class TransactionsControllerTests
    {
        private Random random;
        private ModelGenerator modelGenerator;

        private TransactionsController controller;
        private Mock<ITransactionsService> mockService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();
            this.modelGenerator = new ModelGenerator(random);

            this.mockService = new Mock<ITransactionsService>();
            this.controller = new TransactionsController(this.mockService.Object);
        }
    }
}