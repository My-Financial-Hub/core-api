using Bogus.DataSets;
using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.IntegrationTests.Extensions;
using FinancialHub.Auth.Tests.Common.Assertions;
using FinancialHub.Auth.Tests.Common.Builders.Entities;
using FinancialHub.Domain.Responses.Errors;
using FinancialHub.Domain.Responses.Success;
using System.Net;

/*
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "00-68384f7fb6ba140dbd5d1b80776ba492-59c3ed69db805c8c-00",
  "errors": {
    "Email": [
      "Email é obrigatório",
      "Email é invalido"
    ],
    "LastName": [
      "Last Name é obrigatório"
    ],
    "FirstName": [
      "First Name é obrigatório"
    ]
  }
}
*/

namespace FinancialHub.Auth.IntegrationTests.Controllers.Users
{
    public class UsersControllerTests : BaseControllerTests
    {
        protected UserModelBuilder modelBuilder;
        protected UserEntityBuilder entityBuilder;

        public UsersControllerTests(FinancialHubAuthApiFixture fixture, string? endpoint = null) : 
            base(fixture, "/users" + endpoint)
        {
        }

        [SetUp]
        public override void SetUp()
        {
            modelBuilder = new UserModelBuilder();
            entityBuilder = new UserEntityBuilder();
            base.SetUp();
        }

        public class CreateUser : UsersControllerTests
        {
            public CreateUser(FinancialHubAuthApiFixture fixture) :
                base(fixture)
            {
            }

            [Test]
            public async Task CreateUser_ValidUser_ReturnsCreatedUser()
            {
                var data = modelBuilder.Generate();

                var response = await Client.PostAsync(baseEndpoint, data.ToHttpContent());
                var jsonResponse = await response.ReadContentAsync<SaveResponse<UserModel>>();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonResponse, Is.Not.Null);

                data.Id = jsonResponse.Data.Id;
                ModelAssert.Equal(data, jsonResponse.Data);
            }

            [Test]
            public async Task CreateUser_ValidUser_CreatesUser()
            {
                var data = modelBuilder.Generate();

                var response = await Client.PostAsync(baseEndpoint, data.ToHttpContent());
                var jsonResponse = await response.ReadContentAsync<SaveResponse<UserModel>>();

                var id = jsonResponse!.Data.Id;
                var user = fixture.GetData<UserEntity>().FirstOrDefault(x => x.Id == id);
                Assert.That(user, Is.Not.Null);
            }

            [Test]
            public async Task CreateUser_InvalidUser_ReturnsValidationError()
            {
                var data = modelBuilder
                    .WithName(string.Empty)
                    .WithLastName(string.Empty)
                    .WithEmail(string.Empty)
                    .WithBirthDate(default)
                    .Generate();

                var response = await Client.PostAsync(baseEndpoint, data.ToHttpContent());
                var jsonResponse = await response.ReadContentAsync<ValidationErrorResponse>();

                Assert.That(false);
            }

            [Test]
            public async Task CreateUser_InvalidUser_DoestNotCreateUser()
            {
                var data = modelBuilder
                    .WithName(string.Empty)
                    .WithLastName(string.Empty)
                    .WithEmail(string.Empty)
                    .WithBirthDate(default)
                    .Generate();

                await Client.PostAsync(baseEndpoint, data.ToHttpContent());

                var user = fixture.GetData<UserEntity>().FirstOrDefault();
                Assert.That(user, Is.Null);
            }
        }

        public class GetUser : UsersControllerTests
        {
            public GetUser(FinancialHubAuthApiFixture fixture) : base(fixture)
            {
            }

            [Test]
            public async Task GetUser_NoUsers_Returns404NotFound()
            {
                var id = Guid.NewGuid().ToString();
                var response = await Client.GetAsync(baseEndpoint + $"/{id}");

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public async Task GetUser_ExistingUser_ReturnsUser()
            {
                var id = Guid.NewGuid();
                var entity = entityBuilder.WithId(id).Generate();
                fixture.AddData(entity);

                var response = await Client.GetAsync(baseEndpoint + $"/{id}");
                var jsonResponse = await response.ReadContentAsync<ItemResponse<UserModel>>();

                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(jsonResponse, Is.Not.Null);
                    Assert.That(jsonResponse!.Data.Id, Is.EqualTo(id));
                });
            }
        }
    }
}
