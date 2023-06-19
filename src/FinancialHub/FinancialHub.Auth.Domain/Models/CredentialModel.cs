namespace FinancialHub.Auth.Domain.Models
{
    public class CredentialModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid UserId { get; set; }
    }
}
