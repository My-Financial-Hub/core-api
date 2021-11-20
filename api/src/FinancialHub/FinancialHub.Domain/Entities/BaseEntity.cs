using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Column("id",TypeName = "UNIQUEIDENTIFIER")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Guid? Id { get; set; }

        [Column("creation_time", TypeName = "DATETIMEOFFSET")]
        public DateTimeOffset? CreationTime { get; set; }

        [Column("update_time", TypeName = "DATETIMEOFFSET")]
        public DateTimeOffset? UpdateTime { get; set; }
    }
}
