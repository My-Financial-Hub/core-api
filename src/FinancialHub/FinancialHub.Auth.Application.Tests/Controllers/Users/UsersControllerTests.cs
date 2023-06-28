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

        protected static void AssertValidResponse<T>(BaseResponse<T> expected, BaseResponse<T> response)
        {
            Assert.That(response.Data, Is.EqualTo(expected.Data));
        }

        protected static void AssertErrorResponse<T>(BaseErrorResponse expected, BaseErrorResponse response)
        {
            Assert.Multiple(() =>
            {
                Assert.That(response.Message, Is.EqualTo(expected.Message));
                Assert.That(response.Code, Is.EqualTo(expected.Code));
            });
        }
    }
}
