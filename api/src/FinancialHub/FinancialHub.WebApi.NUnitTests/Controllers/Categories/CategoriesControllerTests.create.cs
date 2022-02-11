using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Responses.Errors;
using FinancialHub.Domain.Responses.Success;
using FinancialHub.Domain.Results.Errors;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class CategoriesControllerTests
    {
        [Test]
        public async Task CreateAccount_Valid_ReturnsOk()
        {
            var body = this.modelGenerator.GenerateCategory();
            var mockResult = new ServiceResult<CategoryModel>(body);

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateCategory(body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<CategoryModel>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<CategoryModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.CreateAsync(body), Times.Once);
        }

        [Test]
        public async Task CreateAccount_Invalid_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.modelGenerator.GenerateCategory();

            var mockResult = new ServiceResult<CategoryModel>(body, new InvalidDataError(errorMessage));

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateCategory(body);

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
