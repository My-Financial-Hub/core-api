using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Services.Configurations;
using FinancialHub.Auth.Services.Services;
using Microsoft.Extensions.Options;

namespace FinancialHub.Auth.Services.Tests.Services
{
    public partial class TokenServiceTests
    {
        private ITokenService service;
        private UserModelBuilder userModelBuilder;

        [SetUp]
        public void SetUp()
        {
            this.userModelBuilder = new UserModelBuilder();
            this.service = new TokenService(Options.Create(DefaultOption));
        }

        private static TokenServiceSettings DefaultOption
        {
            get
            {
                return new TokenServiceSettings()
                {
                    Audience = "https://localhost:5000",
                    Issuer = "https://localhost:5000",
                    SecurityKey = "SecurityKeyVeryBigAndComplexNoOneWouldGuess",
                    Expires = 30
                };
            }
        }
    }
}
