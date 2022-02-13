using FinancialHub.Domain.Models;
using FinancialHub.Domain.Responses.Success;
using FinancialHub.Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class CategoriesControllerTests
    {

        [Test]
        public async Task GetMyCategories_ServiceSuccess_ReturnsOk()
        {
            var mockResult = new ServiceResult<ICollection<CategoryModel>>(
                Enumerable.Repeat(modelGenerator.GenerateCategory(), random.Next(0, 10)).ToArray()
            );

            this.mockService
                .Setup(x => x.GetAllByUserAsync(It.IsAny<string>()))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.GetMyCategories();
            var result = (ObjectResult)response;

            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<ListResponse<CategoryModel>>(result.Value);

            var listResponse = result.Value as ListResponse<CategoryModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.GetAllByUserAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
