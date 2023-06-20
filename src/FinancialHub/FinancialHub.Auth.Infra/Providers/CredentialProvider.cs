namespace FinancialHub.Auth.Infra.Providers
{
    public class CredentialProvider : ICredentialProvider
    {
        private readonly IMapper mapper;
        private readonly ICredentialRepository credentialRepository;

        public CredentialProvider(ICredentialRepository credentialRepository, IMapper mapper)
        {
            this.credentialRepository = credentialRepository;
            this.mapper = mapper;
        }

        public async Task<CredentialModel?> CreateAsync(CredentialModel signup)
        {
            var entity = this.mapper.Map<CredentialEntity>(signup);

            var result = await this.credentialRepository.CreateAsync(entity);
            if(result == null)
            {
                return null;
            }

            return this.mapper.Map<CredentialModel>(result);
        }

        public async Task<CredentialModel?> GetAsync(string email)
        {
            var result = await this.credentialRepository.GetAsync(email);
            if (result == null)
            {
                return null;
            }

            return this.mapper.Map<CredentialModel>(result);
        }
    }
}
