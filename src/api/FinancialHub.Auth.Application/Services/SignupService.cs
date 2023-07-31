namespace FinancialHub.Auth.Application.Services
{
    public class SignupService : ISignupService
    {
        private readonly ISignupProvider signupProvider;
        private readonly ICredentialProvider credentialProvider;

        public SignupService(ISignupProvider signupProvider, ICredentialProvider credentialProvider)
        {
            this.signupProvider = signupProvider;
            this.credentialProvider = credentialProvider;
        }

        public async Task<ServiceResult<UserModel>> CreateAccountAsync(SignupModel signup)
        {
            var credential = await credentialProvider.GetAsync(signup.Email);
            if(credential != null)
                return new ServiceError(400, "Credential already exists");

            var createdUser = await signupProvider.CreateAccountAsync(signup);
            if(createdUser == null)
                return new ServiceError(400, "Failed to create user");

            return createdUser;
        }
    }
}
