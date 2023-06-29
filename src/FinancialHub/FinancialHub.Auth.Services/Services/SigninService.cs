namespace FinancialHub.Auth.Services.Services
{
    public class SigninService : ISigninService
    {
        private readonly ITokenService tokenService;
        private readonly IUserProvider userProvider;

        public SigninService(ITokenService tokenService, IUserProvider userService)
        {
            this.tokenService = tokenService;
            this.userProvider = userService;
        }

        public async Task<ServiceResult<TokenModel>> GenerateToken(SigninModel login)
        {
            var userResult = await this.userProvider.GetAsync(login);

            if (userResult.HasError)
                return userResult.Error;

            return this.tokenService.GenerateToken(userResult.Data);
        }
    }
}
