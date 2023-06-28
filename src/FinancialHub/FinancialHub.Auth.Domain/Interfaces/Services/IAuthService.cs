using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<TokenModel>> GenerateToken(LoginModel login);
    }
}
