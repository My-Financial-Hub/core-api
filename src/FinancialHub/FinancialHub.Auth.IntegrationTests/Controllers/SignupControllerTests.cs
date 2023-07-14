namespace FinancialHub.Auth.IntegrationTests.Controllers
{
    public class SignupControllerTests : BaseControllerTests
    {
        protected SignupModelBuilder signupModelBuilder;

        protected UserCredentialModelBuilder credentialModelBuilder;
        protected UserCredentialEntityBuilder credentialEntityBuilder;

        protected UserModelBuilder userModelBuilder;
        protected UserEntityBuilder userEntityBuilder;

        public SignupControllerTests(FinancialHubAuthApiFixture fixture, string endpoint) 
            : base(fixture, "/signup" + endpoint) { }

        [SetUp]
        public override void SetUp()
        {
            signupModelBuilder = new SignupModelBuilder();

            credentialModelBuilder = new UserCredentialModelBuilder();
            credentialEntityBuilder = new UserCredentialEntityBuilder();

            userModelBuilder = new UserModelBuilder();
            userEntityBuilder = new UserEntityBuilder();

            base.SetUp();
        }

        public class Signup : SignupControllerTests
        {
            public Signup(FinancialHubAuthApiFixture fixture) : base(fixture, string.Empty) { }

            [Test]
            public async Task Signup_ValidData_ReturnsCreatedUser()
            {
                var signup = signupModelBuilder.Generate();
                var user = userModelBuilder
                    .WithName(signup.FirstName)
                    .WithLastName(signup.LastName)
                    .WithEmail(signup.Email)
                    .WithBirthDate(signup.BirthDate)
                    .Generate();

                var response = await Client.PostAsync(baseEndpoint, signup.ToHttpContent());
                var jsonResponse = await response.ReadContentAsync<SaveResponse<UserModel>>();

                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(jsonResponse, Is.Not.Null);
                    user.Id = jsonResponse!.Data.Id;
                    ModelAssert.Equal(user, jsonResponse.Data);
                });
            }

            [Test]
            public async Task Signup_ValidData_AddsUser()
            {
                var signup = signupModelBuilder.Generate();
                var response = await Client.PostAsync(baseEndpoint, signup.ToHttpContent());
                var jsonResponse = await response.ReadContentAsync<SaveResponse<UserModel>>();

                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(jsonResponse, Is.Not.Null);

                    var user = fixture.GetData<UserEntity>().FirstOrDefault(x => x.Id == jsonResponse!.Data.Id);
                    Assert.That(user, Is.Not.Null);
                });
            }

            [Test]
            public async Task Signup_ValidationError_ReturnsError()
            {
                var signup = signupModelBuilder
                    .WithEmail(string.Empty)
                    .WithPassword(string.Empty)
                    .WithConfirmPassword("A")
                    .WithFirstName(string.Empty)
                    .WithLastName(string.Empty)
                    .Generate();

                var response = await Client.PostAsync(baseEndpoint, signup.ToHttpContent());
                var jsonResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                    Assert.That(jsonResponse, Is.Not.Null);
                    Assert.That(jsonResponse?.Errors.Count, Is.EqualTo(5));
                });
            }

            [Test]
            public async Task Signup_ExistingCredential_ReturnsError()
            {
                var signup = signupModelBuilder.Generate();

                var userEntity = userEntityBuilder
                    .WithEmail(signup.Email)
                    .Generate();
                fixture.AddData(userEntity);

                var credentialEntity = credentialEntityBuilder
                    .WithLogin(signup.Email)
                    .WithPassword(signup.Password)
                    .WithUserId(userEntity.Id)
                    .Generate();
                fixture.AddData(credentialEntity);

                var response = await Client.PostAsync(baseEndpoint, signup.ToHttpContent());
                var jsonResponse = await response.ReadContentAsync<ValidationErrorResponse>();

                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                    Assert.That(jsonResponse, Is.Not.Null);
                    Assert.That(jsonResponse!.Message, Is.EqualTo("Credential already exists"));
                });
            }
        }
    }
}
