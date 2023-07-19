using FinancialHub.Auth.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Auth.IntegrationTests.Setup
{
    public partial class FinancialHubAuthApiFixture
    {
        public string GetAuthToken(UserModel userModel)
        {
            using var scope = this.Api.Server.Services.CreateScope();
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            return tokenService.GenerateToken(userModel).Token;
        }
    }
}
