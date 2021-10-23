using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Domain.Entities
{
    [Table("accounts")]
    public class AccountEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("currency")]
        public string Currency { get; set; }
        [Column("active")]
        public bool IsActive { get; set; }
    }
}
