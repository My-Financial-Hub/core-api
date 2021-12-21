using FinancialHub.Domain.Enums;
using FinancialHub.Domain.Model;
using System;

namespace FinancialHub.Domain.Models
{
    public class TransactionModel : BaseModel
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public DateTimeOffset TargetDate { get; set; }
        public DateTimeOffset FinishDate { get; set; }

        public Guid AccountId { get; set; }
        public AccountModel Account { get; set; }

        public Guid CategoryId { get; set; }
        public CategoryModel Category { get; set; }

        public bool IsActive { get; set; }

        public TransactionStatus Status { get; set; }
        public TransactionType Type { get ; set ;}
    }
}
