using Moq;
using NUnit.Framework;
using FinancialHub.WebApi.Controllers;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Tests.Builders.Models;
using System;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class BalancesControllerTests
    {
        private Random random;

        private BalanceModelBuilder balanceModelBuilder;

        private BalancesController controller;
        private Mock<IBalancesService> mockService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();

            this.balanceModelBuilder = new BalanceModelBuilder();

            this.mockService = new Mock<IBalancesService>();
            this.controller = new BalancesController(this.mockService.Object);
        }
    }
}
