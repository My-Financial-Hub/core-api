using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialHub.Domain.Model
{
    public abstract class BaseModel
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("creation_time")]
        public DateTimeOffset CreationDate { get; set; }
        [Column("uptade_time")]
        public DateTimeOffset UpdateDate { get; set; }
    }
}
