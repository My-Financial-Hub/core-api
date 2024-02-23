using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class TransactionDto
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public decimal Amount { get; init; }

        public DateTimeOffset TargetDate { get; init; }
        public DateTimeOffset FinishDate { get; init; }

        public TransactionBalanceDto Balance { get; init; }

        public TransactionCategoryDto Category { get; init; }

        public bool IsActive { get; init; }
        public TransactionStatus Status { get; init; }
        public TransactionType Type { get; init; }
    }
}
