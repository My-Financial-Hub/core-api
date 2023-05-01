using FinancialHub.Auth.WebApi.Controllers;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Tests.Common.Builders.Entities;

namespace FinancialHub.Auth.Application.Tests.Controllers
{
    public partial class UsersControllerTests
    {
        private UsersController controller;
        private Mock<IUserService> serviceMock;
        private UserModelBuilder builder;

        [SetUp] 
        public void SetUp() 
        {
            serviceMock = new Mock<IUserService>();
            controller = new UsersController(serviceMock.Object);
            builder = new UserModelBuilder();
        }
    }
}
