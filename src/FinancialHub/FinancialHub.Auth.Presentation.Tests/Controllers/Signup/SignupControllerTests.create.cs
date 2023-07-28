using FinancialHub.Auth.Presentation.Tests.Asserts;

namespace FinancialHub.Auth.Presentation.Tests.Controllers
{
    public partial class SignupControllerTests
    {
        [Test]
        public async Task CreateAsync_SuccessfulCreation_Returns200Ok()
        {
            var signup = this.builder.Generate();
            var user = this.userBuilder
                .WithName(signup.FirstName)
                .WithLastName(signup.FirstName)
                .WithEmail(signup.Email)
                .WithBirthDate(signup.BirthDate)
                .Generate();
            var expectedResponse = new ItemResponse<UserModel>(user);

            serviceMock
                .Setup(x => x.CreateAccountAsync(signup))
                .ReturnsAsync(user);

            var response = await this.controller.SignupAsync(signup);

            var result = response as ObjectResult;

            Assert.That(result, Is.Not.Null);
            ControllerResponseAssert.IsValid(expectedResponse, result);
        }

        [Test]
        public async Task CreateAsync_FailedCreation_Returns400BadRequest()
        {
            var signup = this.builder.Generate();
            var error = new ServiceError(400, "User data invalid");
            var expectedResponse = new ValidationErrorResponse(error.Message);

            serviceMock
                .Setup(x => x.CreateAccountAsync(signup))
                .ReturnsAsync(error);

            var response = await this.controller.SignupAsync(signup);

            var result = response as ObjectResult;

            Assert.That(result, Is.Not.Null);
            ControllerResponseAssert.HasError(expectedResponse, result);
        }
    }
}
