using System;
using System.Text.Json.Serialization;
using FinancialHub.Common.Models;
using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.Models
{
    public class TransactionModel : BaseModel
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public DateTimeOffset TargetDate { get; set; }
        public DateTimeOffset FinishDate { get; set; }

        public Guid BalanceId { get; set; }
        public BalanceModel Balance { get; set; }

        public Guid CategoryId { get; set; }
        public CategoryModel Category { get; set; }

        public bool IsActive { get; set; }

        public TransactionStatus Status { get; set; }
        public TransactionType Type { get ; set ;}

        [JsonIgnore]
        public bool IsPaid => this.IsActive && this.Status == TransactionStatus.Committed;
    }
}
