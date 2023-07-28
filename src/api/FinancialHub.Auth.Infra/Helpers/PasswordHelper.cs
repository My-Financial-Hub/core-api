using System.Text;
using System.Security.Cryptography;
using FinancialHub.Auth.Domain.Interfaces.Helpers;

namespace FinancialHub.Auth.Infra.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        public string Encrypt(string value)
        {
            var dataArray = Encoding.UTF8.GetBytes(value);

            var bytes = SHA256.Create().ComputeHash(dataArray);

            return new string(
                Convert.ToBase64String(bytes).Reverse().ToArray()
            );
        }

        public bool Verify(string password, string encryptedPassword)
        {
            var encrypted = Encrypt(password);
            return encrypted == password;
        }
    }
}
