using FinancialHub.Common.Entities;

namespace FinancialHub.Auth.Domain.Entities
{
    public class CredentialEntity : BaseEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
