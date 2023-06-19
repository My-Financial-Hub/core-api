﻿namespace FinancialHub.Auth.Infra.Providers
{
    public class CredentialProvider : ICredentialProvider
    {
        private readonly IMapper mapper;
        private readonly ICredentialRepository credentialRepository;

        public CredentialProvider(IMapper mapper,ICredentialRepository credentialRepository)
        {
            this.mapper = mapper;
            this.credentialRepository = credentialRepository;
        }

        public async Task<CredentialModel> CreateAsync(CredentialModel signup)
        {
            var entity = this.mapper.Map<CredentialEntity>(signup);

            var result = await this.credentialRepository.CreateAsync(entity);

            return this.mapper.Map<CredentialModel>(result);
        }

        public async Task<CredentialModel> GetAsync(string email)
        {
            var result = await this.credentialRepository.GetAsync(email);

            return this.mapper.Map<CredentialModel>(result);
        }
    }
}