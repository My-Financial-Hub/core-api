using FinancialHub.Domain.Models;
using FinancialHub.Domain.Responses.Errors;
using FinancialHub.Domain.Responses.Success;
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
        public async Task UpdateAccount_Valid_ReturnsOk()
        {
            var body = this.modelGenerator.GenerateAccount();
            var guid = body.Id.GetValueOrDefault();
            var mockResult = new ServiceResult<AccountModel>(body);

            this.mockService
                .Setup(x => x.UpdateAsync(guid, body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.UpdateAccount(guid, body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<AccountModel>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<AccountModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.UpdateAsync(guid, body), Times.Once);
        }

        [Test]
        public async Task UpdateAccount_Invalid_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.modelGenerator.GenerateAccount();
            var guid = body.Id.GetValueOrDefault();

            var mockResult = new ServiceResult<AccountModel>(body, new InvalidDataError(errorMessage));

            this.mockService
                .Setup(x => x.UpdateAsync(guid,body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.UpdateAccount(guid,body);

            var result = response as ObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
            Assert.IsInstanceOf<ValidationErrorResponse>(result?.Value);

            var listResponse = result?.Value as ValidationErrorResponse;
            Assert.AreEqual(mockResult.Error.Code, listResponse?.Code);
            Assert.AreEqual(mockResult.Error.Message, listResponse?.Message);

            this.mockService.Verify(x => x.UpdateAsync(guid, body), Times.Once);
        }
    }
}
