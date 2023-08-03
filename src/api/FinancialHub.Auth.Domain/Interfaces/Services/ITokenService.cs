using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        TokenModel GenerateToken(UserModel user);
    }
}
