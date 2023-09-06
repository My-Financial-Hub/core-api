namespace FinancialHub.Core.Domain.Entities
{
    public class BalanceEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }

        public Guid AccountId { get; set; }
        public AccountEntity Account { get; set; }

        public ICollection<TransactionEntity> Transactions { get; set; }
    }
}
