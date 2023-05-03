using FinancialHub.Auth.Domain.Models;
using FinancialHub.Domain.Responses.Errors;
using FinancialHub.Domain.Responses.Success;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FinancialHub.Auth.Application.Tests.Controllers
{
    public partial class UsersControllerTests
    {
        [Test]
        public async Task GetUserAsync_FoundUser_Returns200Ok()
        {
            var id = Guid.NewGuid();
            var user = builder
                .WithId(id)
                .Generate();
            var serviceResult = new ServiceResult<UserModel>(user);

            serviceMock
                .Setup(x => x.GetAsync(id))
                .ReturnsAsync(serviceResult);

            var expectedResponse = new ItemResponse<UserModel>(user);

            var response = await controller.GetUserAsync(id);
            var result = response as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(result!.StatusCode, Is.EqualTo(200));
                Assert.That(result!.Value, Is.TypeOf(expectedResponse.GetType()));

                var response = result.Value as ItemResponse<UserModel>;
                AssertValidResponse(expectedResponse, response!);
            });
        }

        [Test]
        public async Task GetUserAsync_NotFoundUser_Returns404NotFound()
        {
            var id = Guid.NewGuid();
            var error = new ServiceError(404, "User not found");
            var serviceResult = new ServiceResult<UserModel>(error: error);

            serviceMock
                .Setup(x => x.GetAsync(id))
                .ReturnsAsync(serviceResult);

            var expectedResponse = new NotFoundErrorResponse(error.Message);

            var response = await controller.GetUserAsync(id);
            var result = response as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(result!.StatusCode, Is.EqualTo(404));
                Assert.That(result!.Value, Is.TypeOf(expectedResponse.GetType()));

                var response = result.Value as NotFoundErrorResponse;
                AssertErrorResponse<NotFoundErrorResponse>(expectedResponse, response!);
            });
        }
    }
}
