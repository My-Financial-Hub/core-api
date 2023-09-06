using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Domain.Interfaces.Providers
{
    public interface ISignupProvider
    {
        Task<UserModel?> CreateAccountAsync(SignupModel signup);
    }
}
