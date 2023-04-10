namespace FinancialHub.Auth.Domain.Models
{
    public class TokenModel
    {
        public string Token { get; }
        public DateTime ExpiresIn { get; }

        public TokenModel(string token)
        {
            Token = token;
        }
    }
}
