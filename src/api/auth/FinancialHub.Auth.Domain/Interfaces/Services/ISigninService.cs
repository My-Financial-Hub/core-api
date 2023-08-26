using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Domain.Interfaces.Services
{
    public interface ISigninService
    {
        Task<ServiceResult<TokenModel>> AuthenticateAsync(SigninModel login);
    }
}
