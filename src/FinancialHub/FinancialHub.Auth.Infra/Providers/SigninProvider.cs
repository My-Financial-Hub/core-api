namespace FinancialHub.Auth.Infra.Providers
{
    public class SigninProvider : ISigninProvider
    {
        private readonly IUserProvider userProvider;
        private readonly ICredentialProvider credentialProvider;
        private readonly IMapper mapper;

        public SigninProvider(ICredentialProvider credentialProvider, IUserProvider userProvider, IMapper mapper)
        {
            this.credentialProvider = credentialProvider;
            this.userProvider = userProvider;
            this.mapper = mapper;
        }

        public async Task<UserModel> GetAccountAsync(SigninModel signin)
        {
            throw new NotImplementedException();
        }
    }
}
