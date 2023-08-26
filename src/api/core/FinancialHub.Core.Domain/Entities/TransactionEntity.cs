using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.Entities
{
    public class TransactionEntity : BaseEntity
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public DateTimeOffset TargetDate { get; set; }
        public DateTimeOffset FinishDate { get; set; }

        public Guid BalanceId { get; set; }
        public BalanceEntity Balance { get; set; }

        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; }

        public bool IsActive { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
    }
}