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
    public partial class AccountsControllerTests
    {

        [Test]
        public async Task GetMyAccounts_ServiceSuccess_ReturnsOk(
            //ServiceResult<IEnumerable<AccountModel>> mockResult
            )
        {
            var mockResult = new ServiceResult<IEnumerable<AccountModel>>(
                Enumerable.Repeat(modelGenerator.GenerateAccount(), random.Next(0, 10))
            );

            this.mockService
                .Setup(x => x.GetAllByUserAsync(It.IsAny<string>()))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.GetMyAccounts();
            var result = (ObjectResult)response;

            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<ListResponse<AccountModel>>(result.Value);

            var listResponse = result.Value as ListResponse<AccountModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.GetAllByUserAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
