namespace FinancialHub.Auth.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService tokenService;
        private readonly IUserService userService;

        public AuthService(ITokenService tokenService, IUserService userService)
        {
            this.tokenService = tokenService;
            this.userService = userService;
        }

        public async Task<ServiceResult<TokenModel>> GenerateToken(LoginModel login)
        {
            var userResult = await this.userService.GetAsync(login);

            if (userResult.HasError)
                return userResult.Error;

            return this.tokenService.GenerateToken(userResult.Data);
        }
    }
}
