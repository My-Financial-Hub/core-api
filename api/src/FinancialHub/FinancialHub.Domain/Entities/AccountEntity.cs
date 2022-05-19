using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Domain.Entities
{
    [Table("accounts")]
    public class AccountEntity : BaseEntity
    {
        [Column("name",TypeName = "varchar(200)")]
        public string Name { get; set; }
        [Column("description", TypeName = "varchar(500)")]
        public string Description { get; set; }
        [Column("active")]
        public bool IsActive { get; set; }

        public ICollection<BalanceEntity> Balances { get; set; }
        public ICollection<TransactionEntity> Transactions { get; set; }
    }
}
