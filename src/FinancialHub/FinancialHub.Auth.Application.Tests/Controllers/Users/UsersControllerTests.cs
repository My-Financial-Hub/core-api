using FinancialHub.Auth.WebApi.Controllers;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Tests.Common.Builders.Entities;
using FinancialHub.Domain.Responses.Success;
using FinancialHub.Domain.Responses.Errors;

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

        public static void AssertValidResponse<T>(BaseResponse<T> expected, BaseResponse<T> response)
        {
            Assert.That(response.Data, Is.EqualTo(expected.Data));
        }

        public static void AssertErrorResponse<T>(BaseErrorResponse expected, BaseErrorResponse response)
        {
            Assert.Multiple(() =>
            {
                Assert.That(response.Message, Is.EqualTo(expected.Message));
                Assert.That(response.Code, Is.EqualTo(expected.Code));
            });
        }
    }
}
