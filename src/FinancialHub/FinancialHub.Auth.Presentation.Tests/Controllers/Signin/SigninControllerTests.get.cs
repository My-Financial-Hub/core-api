using FinancialHub.Auth.Presentation.Tests.Asserts;

namespace FinancialHub.Auth.Presentation.Tests.Controllers
{
    public partial class SigninControllerTests
    {
        [Test]
        public async Task GetAsync_SuccessfulValidation_Returns200Ok()
        {
            var signin = this.builder.Generate();
            var token = this.tokenBuilder.Generate();
            var expectedResponse = new ItemResponse<TokenModel>(token);

            serviceMock
                .Setup(x => x.AuthenticateAsync(signin))
                .ReturnsAsync(token);

            var response = await this.controller.SigninAsync(signin);

            var result = response as ObjectResult;

            Assert.That(result, Is.Not.Null);
            ControllerResponseAssert.IsValid(expectedResponse, result);
        }

        [Test]
        public async Task CreateAsync_ValidationFailed_Returns401Unauthorized()
        {
            var signin = this.builder.Generate();
            var error = new ServiceError(401, "Wrong e-mail or passsword");
            var expectedResponse = new ValidationErrorResponse(error.Message);

            serviceMock
                .Setup(x => x.AuthenticateAsync(signin))
                .ReturnsAsync(error);

            var response = await this.controller.SigninAsync(signin);

            var result = response as ObjectResult;

            Assert.That(result, Is.Not.Null);
            ControllerResponseAssert.HasError(expectedResponse, result, 401);
        }
    }
}
