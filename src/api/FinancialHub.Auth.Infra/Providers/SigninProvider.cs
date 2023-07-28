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

        public async Task<UserModel?> GetAccountAsync(SigninModel signin)
        {
            var credential = mapper.Map<CredentialModel>(signin);
            var existingCredential = await credentialProvider.GetAsync(credential);

            if(existingCredential == null)
            {
                return null;
            }
            
            return await userProvider.GetAsync(existingCredential.UserId);
        }
    }
}
