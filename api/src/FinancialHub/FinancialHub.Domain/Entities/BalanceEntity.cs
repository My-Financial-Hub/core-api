using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Domain.Entities
{
    public class BalanceEntity : BaseEntity
    {
        [Column("name",TypeName = "varchar(200)")]
        public string Name { get; set; }
        [Column("currency",TypeName = "varchar(50)")]
        public string Currency { get; set; }
        [Column("amount", TypeName = "money")]
        public decimal Amount { get; set; }

        [Column("account_id")]
        public Guid AccountId { get; set; }
        public AccountEntity Account { get; set; }
    }
}
