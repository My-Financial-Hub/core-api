using System.Text.Json.Serialization;
using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.Models
{
    public record class TransactionModel : BaseModel
    {
        public string Description { get; private set; }
        public decimal Amount { get; private set; }

        public DateTimeOffset TargetDate { get; private set; }
        public DateTimeOffset FinishDate { get; private set; }

        public Guid BalanceId { get; private set; }
        public BalanceModel Balance { get; private set; }

        public Guid CategoryId { get; private set; }
        public CategoryModel Category { get; private set; }

        public bool IsActive { get; private set; }

        public TransactionStatus Status { get; private set; }
        public TransactionType Type { get ; private set; }

        [JsonIgnore]
        public bool IsPaid => this.IsActive && this.Status == TransactionStatus.Committed;
    }
}
