namespace FinancialHub.Auth.Presentation.Tests.Controllers
{
    public partial class SignupControllerTests
    {
        private SignupController controller;
        private Mock<ISignupService> serviceMock;
        private SignupModelBuilder builder;
        private UserModelBuilder userBuilder;

        [SetUp]
        public void SetUp()
        {
            builder = new SignupModelBuilder();
            userBuilder = new UserModelBuilder();
            serviceMock = new Mock<ISignupService>();
            controller = new SignupController(serviceMock.Object);
        }
    }
}
