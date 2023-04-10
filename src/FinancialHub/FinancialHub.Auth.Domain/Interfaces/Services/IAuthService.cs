using FinancialHub.Auth.Domain.Models;
using FinancialHub.Domain.Results;

namespace FinancialHub.Auth.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<TokenModel>> GenerateToken(LoginModel login);
    }
}
