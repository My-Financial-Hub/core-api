using System.Collections.Generic;
using FinancialHub.Common.Entities;

namespace FinancialHub.Domain.Entities
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<TransactionEntity> Transactions { get; set; }
    }
}
