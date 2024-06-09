namespace FinancialHub.Core.Domain.Models
{
    public class BalanceModel : BaseModel
    {
        public string Name { get; init; }
        public string Currency { get; init; }
        public decimal Amount { get; init; }
        public Guid AccountId { get; init; }
        public AccountModel Account { get; init; }
        public bool IsActive { get; init; }
    }
}
