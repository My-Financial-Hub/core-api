using FinancialHub.Domain.Models;
using FinancialHub.Common.Results;
using FinancialHub.Common.Responses.Errors;
using FinancialHub.Common.Responses.Success;
using FinancialHub.Common.Results.Errors;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class BalancesControllerTests
    {
        [Test]
        public async Task CreateBalance_ServiceSuccess_ReturnsOk()
        {
            var body = this.balanceModelBuilder.Generate();
            var mockResult = new ServiceResult<BalanceModel>(body);

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateBalance(body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<BalanceModel>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<BalanceModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.CreateAsync(body), Times.Once);
        }

        [Test]
        [TestCase(Description = "Create account returns Bad Request", Category = "Create")]
        public async Task CreateBalance_ServiceError_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.balanceModelBuilder.Generate();

            var mockResult = new ServiceResult<BalanceModel>(body, new InvalidDataError(errorMessage));

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateBalance(body);

            var result = response as ObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
            Assert.IsInstanceOf<ValidationErrorResponse>(result?.Value);

            var listResponse = result?.Value as ValidationErrorResponse;
            Assert.AreEqual(mockResult.Error.Code, listResponse?.Code);
            Assert.AreEqual(mockResult.Error.Message, listResponse?.Message);

            this.mockService.Verify(x => x.CreateAsync(body), Times.Once);
        }
    }
}
