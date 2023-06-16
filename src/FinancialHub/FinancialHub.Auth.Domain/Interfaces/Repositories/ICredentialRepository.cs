using FinancialHub.Auth.Domain.Entities;

namespace FinancialHub.Auth.Domain.Interfaces.Repositories
{
    public interface ICredentialRepository
    {
        Task<int> DeleteAsync(string username, string password);
        Task<int> DeleteAsync(Guid userId);
        Task<CredentialEntity?> GetAsync(string username);
        Task<CredentialEntity?> GetAsync(string username, string password); 
        Task<CredentialEntity?> GetAsync(Guid userId);
        Task<CredentialEntity> CreateAsync(CredentialEntity credential);
    }
}
