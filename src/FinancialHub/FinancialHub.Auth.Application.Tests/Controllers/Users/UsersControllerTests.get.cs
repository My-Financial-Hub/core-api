using FinancialHub.Auth.Domain.Models;
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

            serviceMock
                .Setup(x => x.GetAsync(id))
                .ReturnsAsync(user);

            var response = await controller.GetUserAsync(id);

            var result = response as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result!.Value, Is.TypeOf(typeof(UserModel)).And.EqualTo(user));
        }

        [Test]
        public async Task GetUserAsync_NotFoundUser_Returns404NotFound()
        {
            var id = Guid.NewGuid();
            var response = await controller.GetUserAsync(id);
        }
    }
}
