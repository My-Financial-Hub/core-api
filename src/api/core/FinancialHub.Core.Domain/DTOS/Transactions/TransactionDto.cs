using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class TransactionDto
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public decimal Amount { get; private set; }

        public DateTimeOffset TargetDate { get; private set; }
        public DateTimeOffset FinishDate { get; private set; }

        public TransactionBalanceDto Balance { get; private set; }

        public TransactionCategoryDto Category { get; private set; }

        public TransactionStatus Status { get; private set; }
        public TransactionType Type { get; private set; }
    }
}
