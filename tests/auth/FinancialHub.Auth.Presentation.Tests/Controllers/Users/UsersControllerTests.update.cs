namespace FinancialHub.Auth.Presentation.Tests.Controllers
{
    public partial class UsersControllerTests
    {
        [Test]
        public async Task UpdateAsync_ExistingUser_Returns200Ok()
        {
            var id = Guid.NewGuid();
            var user = this.builder.WithId(id).Generate();
            var serviceResult = new ServiceResult<UserModel>(user);
            var expectedResponse = new SaveResponse<UserModel>(user);

            serviceMock
                .Setup(x => x.UpdateAsync(id, user))
                .ReturnsAsync(serviceResult);

            var response = await this.controller.UpdateUserAsync(id, user);
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
        public async Task UpdateAsync_NonExistingUser_Returns404NotFound()
        {
            var id = Guid.NewGuid();
            var user = this.builder.WithId(id).Generate();

            var error = new ServiceError(404, "User data invalid");
            var serviceResult = new ServiceResult<UserModel>(error: error);
            var expectedResponse = new NotFoundErrorResponse(error.Message);

            serviceMock
                .Setup(x => x.UpdateAsync(id, user))
                .ReturnsAsync(serviceResult);

            var response = await this.controller.UpdateUserAsync(id, user);
            var result = response as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(result!.StatusCode, Is.EqualTo(404));
                Assert.That(result!.Value, Is.TypeOf(expectedResponse.GetType()));

                var response = result.Value as NotFoundErrorResponse;
                AssertErrorResponse<NotFoundErrorResponse>(expectedResponse, response!);
            });
        }

        [Test]
        public async Task UpdateAsync_UpdateError_Returns400BadRequest()
        {
            var id = Guid.NewGuid();
            var user = this.builder.WithId(id).Generate();

            var error = new ServiceError(400, "User data invalid");
            var serviceResult = new ServiceResult<UserModel>(error: error);
            var expectedResponse = new ValidationErrorResponse(error.Message);

            serviceMock
                .Setup(x => x.UpdateAsync(id, user))
                .ReturnsAsync(serviceResult);

            var response = await this.controller.UpdateUserAsync(id, user);
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
