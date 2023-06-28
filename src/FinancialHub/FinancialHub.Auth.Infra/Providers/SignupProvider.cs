namespace FinancialHub.Auth.Infra.Providers
{
    public class SignupProvider : ISignupProvider
    {
        private readonly ICredentialProvider credentialProvider;
        private readonly IUserProvider userProvider;
        private readonly IMapper mapper;

        public SignupProvider(
            ICredentialProvider credentialProvider,
            IUserProvider userProvider,
            IMapper mapper
        )
        {
            this.credentialProvider = credentialProvider;
            this.userProvider = userProvider;
            this.mapper = mapper;
        }

        public async Task<UserModel?> CreateAccountAsync(SignupModel signup)
        {
            var createdUser = await userProvider.CreateAsync(
                mapper.Map<UserModel>(signup)
            );
            if(createdUser == null)
                return null;

            var userCredential = mapper.Map<CredentialModel>(signup);
            userCredential.UserId = createdUser.Id.GetValueOrDefault();

            var createdCredential = await credentialProvider.CreateAsync(userCredential);
            if (createdCredential == null)
                return null;

            return createdUser;
        }
    }
}
