using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Domain.Entities
{
    [Table("categories")]
    public class CategoryEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("active")]
        public bool IsActive { get; set; }

        public ICollection<TransactionEntity> Transactions { get; set; }
    }
}
