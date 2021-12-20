using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Domain.Entities
{
    [Table("transactions")]
    public class TransactionEntity : BaseEntity
    {
        [Column("description")]
        public string Description { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        [Column("target_date")]
        public DateTimeOffset TargetDate { get; set; }
        [Column("finish_date")]
        public DateTimeOffset FinishDate { get; set; }

        [Column("account_id")]
        public Guid AccountId { get; set; }
        public AccountEntity Account { get; set; }

        [Column("category_id")]
        [ForeignKey("category_id")]
        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; }

        [Column("active")]
        public bool IsActive { get; set; }

        [Column("status")]
        public int Status { get; set; }
        //TODO: use enum
        [Column("type")]
        public int Type { get ; set ;}
    }
}