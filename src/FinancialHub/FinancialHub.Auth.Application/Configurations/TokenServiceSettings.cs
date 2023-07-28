namespace FinancialHub.Auth.Application.Configurations
{
    public class TokenServiceSettings
    {
        public string Audience { get; set ; }
        public string Issuer { get; set ; }
        public string SecurityKey { get; set; }
        public int Expires { get; set; }
    }
}
