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
            var user = await this.signinProvider.GetAccountAsync(login);

            if (user == null)
                return new ServiceError(401, "Wrong e-mail or password");

            return this.tokenService.GenerateToken(user);
        }
    }
}
