namespace FinancialHub.Auth.Application.Tests.Controllers
{
    public partial class SigninControllerTests
    {
        private SigninController controller;
        private Mock<ISigninService> serviceMock;
        private SigninModelBuilder builder;
        private TokenModelBuilder tokenBuilder;

        [SetUp]
        public void SetUp()
        {
            builder = new SigninModelBuilder();
            tokenBuilder = new TokenModelBuilder();
            serviceMock = new Mock<ISigninService>();
            controller = new SigninController(serviceMock.Object);
        }
    }
}
