namespace FinancialHub.Auth.Domain.Interfaces.Helpers
{
    //TODO: use IPasswordHasher<T> and IPasswordValidator
    public interface IPasswordHelper
    {
        string Encrypt(string value);
        bool Verify(string password, string encryptedPassword);
    }
}
