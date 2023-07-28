using System.Diagnostics.CodeAnalysis;

namespace FinancialHub.Auth.Presentation.Tests.Controllers
{
    public partial class UsersControllerTests
    {
        [Test]
        [Obsolete("Obsolete while CreateUserAsync is Obsolete")]
        [SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed", Justification = "Remove with CreateUserAsync")]
        public async Task CreateAsync_SuccessfulCreation_Returns200Ok()
        {
            var user = this.builder.Generate();
            var expectedResponse = new SaveResponse<UserModel>(user);

            serviceMock
                .Setup(x => x.CreateAsync(user))
                .ReturnsAsync(user);

            var response = await this.controller.CreateUserAsync(user);

            var result = response as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(result!.StatusCode, Is.EqualTo(200));
                Assert.That(result!.Value, Is.TypeOf(expectedResponse.GetType()));

                var response = result.Value as SaveResponse<UserModel>;
                AssertValidResponse(expectedResponse, response!);
            });
        }

        [Test]
        [Obsolete("Obsolete while CreateUserAsync is Obsolete")]
        [SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed", Justification = "Remove with CreateUserAsync")]
        public async Task CreateAsync_FailedCreation_Returns400BadRequest()
        {
            var user = this.builder.Generate();
            var error = new ServiceError(400, "User data invalid");
            var serviceResult = new ServiceResult<UserModel>(error: error);

            serviceMock
                .Setup(x => x.CreateAsync(user))
                .ReturnsAsync(serviceResult);

            var expectedResponse = new ValidationErrorResponse(error.Message);

            var response = await this.controller.CreateUserAsync(user);

            var result = response as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(result!.StatusCode, Is.EqualTo(400));
                Assert.That(result!.Value, Is.TypeOf(expectedResponse.GetType()));

                var response = result.Value as ValidationErrorResponse;
                AssertErrorResponse<ValidationErrorResponse>(expectedResponse, response!);
            });
        }
    }
}
