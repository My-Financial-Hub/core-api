using FinancialHub.Domain.Filters;
using FinancialHub.Domain.Models;
using FinancialHub.Common.Responses.Success;
using FinancialHub.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class TransactionsControllerTests
    {
        [Test]
        [TestCase(Description = "Get Transactions return Ok", Category = "Create")]
        public async Task GetMyTransactions_ServiceSuccess_ReturnsOk()
        {
            var mockResult = new ServiceResult<ICollection<TransactionModel>>(transactionModelBuilder.Generate(random.Next(0, 10)));

            var filter = new TransactionFilter();

            this.mockService
                .Setup(x => x.GetAllByUserAsync(It.IsAny<string>(),filter))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.GetMyTransactions(filter);
            var result = (ObjectResult)response;

            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<ListResponse<TransactionModel>>(result.Value);

            var listResponse = result.Value as ListResponse<TransactionModel>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.GetAllByUserAsync(It.IsAny<string>(),filter), Times.Once);
        }
    }
}
