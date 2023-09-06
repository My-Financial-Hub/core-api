using FinancialHub.Auth.Domain.Interfaces.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace FinancialHub.Auth.IntegrationTests.Controllers
{
    public class SigninControllerTests : BaseControllerTests
    {
        protected UserCredentialEntityBuilder credentialEntityBuilder;
        protected UserEntityBuilder userEntityBuilder;

        protected SigninModelBuilder signinModelBuilder;

        public SigninControllerTests(FinancialHubAuthApiFixture fixture,string endpoint) 
            : base(fixture, "/signin" + endpoint)
        {
        }

        [SetUp]
        public override void SetUp()
        {
            credentialEntityBuilder = new UserCredentialEntityBuilder();
            userEntityBuilder = new UserEntityBuilder();

            signinModelBuilder = new SigninModelBuilder();

            base.SetUp();
        }
        public class Signin : SigninControllerTests
        {
            public Signin(FinancialHubAuthApiFixture fixture) : base(fixture, string.Empty){ }

            [Test]
            public async Task Signin_ValidData_ReturnsValidToken()
            {
                var now = DateTime.UtcNow;
                using var scope = this.fixture.Api.Services.CreateScope();

                var id = Guid.NewGuid();
                var user = userEntityBuilder.WithId(id).Generate();
                fixture.AddData(user);

               var passwordHelper = scope.ServiceProvider.GetRequiredService<IPasswordHelper>();

                var password = Guid.NewGuid().ToString();
                var encryptedPassword = passwordHelper.Encrypt(password);
                var credential = credentialEntityBuilder
                    .WithUserId(id)
                    .WithLogin(user.Email)
                    .WithPassword(encryptedPassword)
                    .Generate();
                fixture.AddData(credential);

                var signin = signinModelBuilder
                    .WithEmail(user.Email)
                    .WithPassword(password)
                    .Generate();

                var response = await Client.PostAsync(baseEndpoint, signin.ToHttpContent());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var jsonResponse = await response.ReadContentAsync<SaveResponse<TokenModel>>();
                Assert.Multiple(() =>
                {
                    Assert.That(jsonResponse?.Data.ExpiresIn, Is.InRange(now.AddMinutes(55), now.AddMinutes(65)));

                    var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jsonResponse?.Data.Token);
                    Assert.That(jwt.Payload.Jti, Is.EqualTo(id.ToString()));
                    Assert.That(jwt.Payload.FirstOrDefault(x => x.Key == "email").Value , Is.EqualTo(user.Email));
                });
            }

            [Test]
            public async Task Signin_ValidationError_ReturnsError()
            {
                var signin = signinModelBuilder
                    .WithEmail(string.Empty)
                    .WithPassword(string.Empty)
                    .Generate();

                var response = await Client.PostAsync(baseEndpoint, signin.ToHttpContent());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                
                var jsonResponse = await response.ReadContentAsync<ValidationsErrorResponse>();
                Assert.Multiple(() =>
                {
                    Assert.That(jsonResponse, Is.Not.Null);
                    Assert.That(jsonResponse?.Errors!.Count, Is.EqualTo(2));
                });
            }

            [Test]
            public async Task Signin_NotExitingCredential_DoesNotReturnToken()
            {
                using(var scope = this.fixture.Api.Services.CreateScope())
                {
                    var id = Guid.NewGuid();
                    var user = userEntityBuilder.WithId(id).Generate();
                    fixture.AddData(user);

                    var passwordHelper = scope.ServiceProvider.GetRequiredService<IPasswordHelper>();

                    var password = Guid.NewGuid().ToString();
                    var encryptedPassword = passwordHelper.Encrypt(password);
                    var credential = credentialEntityBuilder
                        .WithUserId(id)
                        .WithLogin(user.Email)
                        .WithPassword(encryptedPassword)
                        .Generate();
                    fixture.AddData(credential);
                }

                var signin = signinModelBuilder.Generate();

                var response = await Client.PostAsync(baseEndpoint, signin.ToHttpContent());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));

                var jsonResponse = await response.ReadContentAsync<ValidationsErrorResponse>();
                Assert.Multiple(() =>
                {
                    Assert.That(jsonResponse, Is.Not.Null);
                    Assert.That(jsonResponse?.Message, Is.EqualTo("Wrong e-mail or password"));
                });
            }

            [Test]
            public async Task Signin_InvalidCredential_DoesNotReturnToken()
            {
                using var scope = this.fixture.Api.Services.CreateScope();
                var id = Guid.NewGuid();
                var user = userEntityBuilder.WithId(id).Generate();
                fixture.AddData(user);

                var passwordHelper = scope.ServiceProvider.GetRequiredService<IPasswordHelper>();

                var password = Guid.NewGuid().ToString();
                var encryptedPassword = passwordHelper.Encrypt(password);
                var credential = credentialEntityBuilder
                    .WithUserId(id)
                    .WithLogin(user.Email)
                    .WithPassword(encryptedPassword)
                    .Generate();
                fixture.AddData(credential);

                var signin = signinModelBuilder
                    .WithEmail(user.Email)
                    .Generate();

                var response = await Client.PostAsync(baseEndpoint, signin.ToHttpContent());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));

                var jsonResponse = await response.ReadContentAsync<ValidationsErrorResponse>();
                Assert.Multiple(() => 
                { 
                    Assert.That(jsonResponse, Is.Not.Null);
                    Assert.That(jsonResponse?.Message, Is.EqualTo("Wrong e-mail or password"));
                });
            }
        }
    }
}
