using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class UpdateTransactionDto
    {
        public string Description { get; init; }
        public decimal Amount { get; init; }

        public DateTimeOffset TargetDate { get; init; }
        public DateTimeOffset FinishDate { get; init; }

        public Guid BalanceId { get; init; }

        public Guid CategoryId { get; init; }

        public bool IsActive { get; init; }
        public TransactionStatus Status { get; init; }
        public TransactionType Type { get; init; }
    }
}
