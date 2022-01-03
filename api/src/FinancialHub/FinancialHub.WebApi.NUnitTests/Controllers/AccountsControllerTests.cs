using Moq;
using NUnit.Framework;
using FinancialHub.WebApi.Controllers;
using FinancialHub.Domain.Interfaces.Services;
using System.Threading.Tasks;
using FinancialHub.Domain.Results;
using System.Collections.Generic;
using FinancialHub.Domain.Models;
using System.Linq;
using System;
using FinancialHub.Domain.NUnitTests.Generators;
using Microsoft.AspNetCore.Mvc;
using FinancialHub.Domain.Responses;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public class AccountsControllerTests
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

        [Test]
        public async Task GetMyAccounts_ServiceSuccess_Returns200()
        {
            var mockResult = new ServiceResult<IEnumerable<AccountModel>>()
            {
                Data = Enumerable.Repeat(modelGenerator.GenerateAccount(), random.Next(0,10))
            };

            this.mockService
                .Setup(x => x.GetAllByUserAsync(It.IsAny<string>()))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.GetMyAccounts();
            var result = (ObjectResult)response;

            Assert.IsNotNull(result);
            Assert.AreEqual(200,result.StatusCode);
            Assert.IsInstanceOf<ListResponse<AccountModel>>(result.Value);

            var listResponse = result.Value as ListResponse<AccountModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.GetAllByUserAsync(It.IsAny<string>()),Times.Once);
        }
    }
}
