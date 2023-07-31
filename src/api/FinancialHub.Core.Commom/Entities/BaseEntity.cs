using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Common.Entities
{
    public abstract class BaseEntity
    {
        [Column("id")]
        public Guid? Id { get; set; }

        [Column("creation_time")]
        public DateTimeOffset? CreationTime { get; set; }

        [Column("update_time")]
        public DateTimeOffset? UpdateTime { get; set; }
    }
}
