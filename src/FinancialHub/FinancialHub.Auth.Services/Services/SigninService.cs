namespace FinancialHub.Auth.Services.Services
{
    public class SigninService : ISigninService
    {
        private readonly ITokenService tokenService;
        private readonly ISigninProvider signinProvider;

        public SigninService(ITokenService tokenService, ISigninProvider signinProvider)
        {
            this.tokenService = tokenService;
            this.signinProvider = signinProvider;
        }

        public async Task<ServiceResult<TokenModel>> AuthenticateAsync(SigninModel login)
        {
            var uer = await this.signinProvider.GetAccountAsync(login);

            if (uer == null)
                return new ServiceError(401, "Failed to login");

            return this.tokenService.GenerateToken(uer);
        }
    }
}
