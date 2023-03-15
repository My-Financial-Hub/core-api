using System;
using FinancialHub.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Domain.Entities
{
    [Table("transactions")]
    public class TransactionEntity : BaseEntity
    {
        [Column("description", TypeName = "varchar(500)")]
        public string Description { get; set; }

        [Column("amount",TypeName = "money")]
        public decimal Amount { get; set; }

        [Column("target_date")]
        public DateTimeOffset TargetDate { get; set; }
        [Column("finish_date")]
        public DateTimeOffset FinishDate { get; set; }

        [Column("balance_id")]
        public Guid BalanceId { get; set; }
        public BalanceEntity Balance { get; set; }

        [Column("category_id")]
        [ForeignKey("category_id")]
        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; }

        [Column("active")]
        public bool IsActive { get; set; }

        [Column("status")]
        public TransactionStatus Status { get; set; }
        [Column("type")]
        public TransactionType Type { get; set; }
    }
}