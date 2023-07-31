namespace FinancialHub.Auth.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        TokenModel GenerateToken(UserModel user);
    }
}
