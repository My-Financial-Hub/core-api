using Moq;
using NUnit.Framework;
using FinancialHub.WebApi.Controllers;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Tests.Builders.Models;
using System;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    //TODO: create default controller test 
    //TODO: create integration test for controllers

    //TODO: start to use : 
    //https://docs.educationsmediagroup.com/unit-testing-csharp/nunit/parameterized-tests 
    //or
    //https://docs.nunit.org/articles/nunit/writing-tests/attributes/theory.html
    public partial class AccountsControllerTests
    {
        private Random random;

        private AccountModelBuilder accountModelBuilder;

        private AccountsController controller;
        private Mock<IAccountsService> mockService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();

            this.accountModelBuilder = new AccountModelBuilder();

            this.mockService = new Mock<IAccountsService>();
            this.controller = new AccountsController(this.mockService.Object);
        }
    }
}
