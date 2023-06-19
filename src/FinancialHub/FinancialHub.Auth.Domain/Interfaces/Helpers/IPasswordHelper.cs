namespace FinancialHub.Auth.Domain.Interfaces.Helpers
{
    public interface IPasswordHelper
    {
        string Encrypt(string value);
        bool Verify(string password, string encryptedPassword);
    }
}
