using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class UpdateTransactionDto
    {
        public string Description { get; private set; }
        public decimal Amount { get; private set; }

        public DateTimeOffset TargetDate { get; private set; }
        public DateTimeOffset FinishDate { get; private set; }

        public Guid BalanceId { get; private set; }

        public Guid CategoryId { get; private set; }

        public bool IsActive { get; private set; }
        public TransactionStatus Status { get; private set; }
        public TransactionType Type { get; private set; }

        public UpdateTransactionDto()
        {
            
        }

        public UpdateTransactionDto(string description, decimal amount, DateTimeOffset targetDate, DateTimeOffset finishDate, Guid balanceId, Guid categoryId, bool isActive, TransactionStatus status, TransactionType type)
        {
            Description = description;
            Amount = amount;
            TargetDate = targetDate;
            FinishDate = finishDate;
            BalanceId = balanceId;
            CategoryId = categoryId;
            IsActive = isActive;
            Status = status;
            Type = type;
        }
    }
}
