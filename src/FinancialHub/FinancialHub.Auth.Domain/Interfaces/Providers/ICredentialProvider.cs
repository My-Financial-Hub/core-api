using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Domain.Interfaces.Providers
{
    public interface ICredentialProvider
    {
        Task<CredentialModel?> CreateAsync(CredentialModel signup);
        Task<CredentialModel?> GetAsync(string email);
    }
}
