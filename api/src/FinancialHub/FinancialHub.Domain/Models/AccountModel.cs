using FinancialHub.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Domain.Models
{
    [Table("accounts")]
    public class AccountModel : BaseModel
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
