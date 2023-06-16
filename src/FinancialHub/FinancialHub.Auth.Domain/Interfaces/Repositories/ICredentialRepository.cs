using FinancialHub.Auth.Domain.Entities;

namespace FinancialHub.Auth.Domain.Interfaces.Repositories
{
    public interface ICredentialRepository
    {
        Task<CredentialEntity?> GetAsync(string username);
        Task<CredentialEntity?> GetAsync(string username, string password); 
        Task<IEnumerable<CredentialEntity>> GetAsync(Guid userId);
        Task<CredentialEntity> CreateAsync(CredentialEntity credential);
    }
}
