using FinancialHub.Domain.Models;
using FinancialHub.Domain.Responses;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Results.Errors;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class AccountsControllerTests
    {
        [Test]
        public async Task CreateAccount_Valid_ReturnsOk()
        {
            var body = this.modelGenerator.GenerateAccount();
            var mockResult = new ServiceResult<AccountModel>(body);

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateAccount(body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<AccountModel>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<AccountModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.CreateAsync(body), Times.Once);
        }

        [Test]
        public async Task CreateAccount_Invalid_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.modelGenerator.GenerateAccount();

            var mockResult = new ServiceResult<AccountModel>(body, new InvalidDataError(errorMessage));

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateAccount(body);

            var result = response as ObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
            Assert.IsInstanceOf<ValidationErrorResponse<AccountModel>>(result?.Value);

            var listResponse = result?.Value as ValidationErrorResponse<AccountModel>;
            Assert.IsNull(listResponse?.Data);
            Assert.AreEqual(mockResult.Error.Code, listResponse?.Error.Code);
            Assert.AreEqual(mockResult.Error.Message, listResponse?.Error.Message);

            this.mockService.Verify(x => x.CreateAsync(body), Times.Once);
        }
    }
}
