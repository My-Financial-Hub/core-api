using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.IntegrationTests.Extensions;
using FinancialHub.Auth.Tests.Common.Assertions;
using FinancialHub.Auth.Tests.Common.Builders.Entities;
using FinancialHub.Auth.Tests.Common.Builders.Models;
using FinancialHub.Domain.Responses.Errors;
using FinancialHub.Domain.Responses.Success;
using System.Net;

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

                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(jsonResponse, Is.Not.Null);
                    data.Id = jsonResponse!.Data.Id;
                    ModelAssert.Equal(data, jsonResponse.Data);
                });
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
                var jsonResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

                Assert.That(jsonResponse?.Errors.Count, Is.EqualTo(4));
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
            public async Task GetUser_NotExistingUser_ReturnsNotFound()
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

        public class UpdateUser : UsersControllerTests
        {
            public UpdateUser(FinancialHubAuthApiFixture fixture) : base(fixture)
            {
            }

            [Test]
            public async Task UpdateUser_ExistingUser_UpdatesUser()
            {
                var id = Guid.NewGuid();
                var entity = entityBuilder.WithId(id).Generate();
                fixture.AddData(entity);

                var data = modelBuilder.WithId(id).Generate();
                await Client.PatchAsync(baseEndpoint + $"/{id}", data.ToHttpContent());

                var databaseUsers = fixture.GetData<UserEntity>();
                var databaseUser = databaseUsers.FirstOrDefault(x => x.Id == id);
                Assert.Multiple(() =>
                {
                    Assert.That(databaseUsers.Count(), Is.EqualTo(1));
                    Assert.That(databaseUser, Is.Not.Null);
                    EntityAssert.Equal(databaseUser!, data);
                });
            }

            [Test]
            public async Task UpdateUser_ExistingUser_ReturnsUpdatedValues()
            {
                var id = Guid.NewGuid();
                var entity = entityBuilder.WithId(id).Generate();
                fixture.AddData(entity);

                var data = modelBuilder.WithId(id).Generate();
                var response = await Client.PatchAsync(baseEndpoint + $"/{id}", data.ToHttpContent());
                var jsonResponse = await response.ReadContentAsync<SaveResponse<UserModel>>();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                ModelAssert.Equal(data, jsonResponse!.Data);
            }

            [Test]
            public async Task UpdateUser_InvalidUser_ReturnsValidationError()
            {
                var id = Guid.NewGuid();
                var entity = entityBuilder.WithId(id).Generate();
                fixture.AddData(entity);

                var data = modelBuilder
                    .WithName(string.Empty)
                    .WithLastName(string.Empty)
                    .WithEmail(string.Empty)
                    .WithBirthDate(default)
                    .WithId(id)
                    .Generate();
                var response = await Client.PatchAsync(baseEndpoint + $"/{id}", data.ToHttpContent());
                var jsonResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

                Assert.That(jsonResponse?.Errors.Count, Is.EqualTo(4));
            }

            [Test]
            public async Task UpdateUser_InvalidUser_DoesNotUpdate()
            {
                var id = Guid.NewGuid();
                var data = modelBuilder
                    .WithName(string.Empty)
                    .WithLastName(string.Empty)
                    .WithEmail(string.Empty)
                    .WithBirthDate(default)
                    .WithId(id)
                    .Generate();

                await Client.PatchAsync(baseEndpoint + $"/{id}", data.ToHttpContent());

                var databaseUsers = fixture.GetData<UserEntity>();
                var databaseUser = databaseUsers.FirstOrDefault(x => x.Id == id);
                Assert.Multiple(() =>
                {
                    Assert.That(databaseUsers.Count(), Is.EqualTo(0));
                    Assert.That(databaseUser, Is.Null);
                });
            }

            [Test]
            public async Task UpdateUser_NotExistingUser_ReturnsNotFound()
            {
                var id = Guid.NewGuid();
                var data = modelBuilder.WithId(id).Generate();
                var response = await Client.PatchAsync(baseEndpoint + $"/{id}", data.ToHttpContent());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public async Task UpdateUser_NotExistingUser_DoesNotUpdate()
            {
                var id = Guid.NewGuid();
                var data = modelBuilder.WithId(id).Generate();
                await Client.PatchAsync(baseEndpoint + $"/{id}", data.ToHttpContent());

                var databaseUsers = fixture.GetData<UserEntity>();
                var databaseUser = databaseUsers.FirstOrDefault(x => x.Id == id);
                Assert.Multiple(() =>
                {
                    Assert.That(databaseUsers.Count(), Is.EqualTo(0));
                    Assert.That(databaseUser, Is.Null);
                });
            }
        }
    }
}
