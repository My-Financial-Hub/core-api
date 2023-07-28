using FinancialHub.Domain.Entities;

namespace FinancialHub.Auth.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
    }
}
