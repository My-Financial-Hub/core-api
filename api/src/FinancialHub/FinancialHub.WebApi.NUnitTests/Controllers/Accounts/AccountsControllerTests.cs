using Moq;
using System;
using NUnit.Framework;
using FinancialHub.WebApi.Controllers;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.NUnitTests.Generators;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    //TODO: start to use : 
    //https://docs.educationsmediagroup.com/unit-testing-csharp/nunit/parameterized-tests 
    //or
    //https://docs.nunit.org/articles/nunit/writing-tests/attributes/theory.html
    public partial class AccountsControllerTests
    {
        private Random random;
        private ModelGenerator modelGenerator;

        private AccountsController controller;
        private Mock<IAccountsService> mockService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();
            this.modelGenerator = new ModelGenerator(random);

            this.mockService = new Mock<IAccountsService>();
            this.controller = new AccountsController(this.mockService.Object);
        }
    }
}
