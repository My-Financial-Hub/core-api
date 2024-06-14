using FinancialHub.Common.Models;
using FinancialHub.Core.Domain.Balances.Enums;

namespace FinancialHub.Core.Domain.Balances.Models
{
    public class Transaction : BaseModel
    {
        public string Description { get; private set; }
        public decimal Amount { get; private set; }

        public DateTimeOffset TargetDate { get; private set; }
        public DateTimeOffset FinishDate { get; private set; }

        public Category Category { get; private set; }

        public bool IsActive { get; private set; }

        public TransactionStatus Status { get; private set; }
        public TransactionType Type { get ; private set; }

        public bool IsPaid => this.IsActive && this.Status == TransactionStatus.Committed;
    }
}
