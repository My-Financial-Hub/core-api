namespace FinancialHub.Core.Domain.DTOS.Balances
{
    public class CreateBalanceDto
    {
        public string Name { get; init; }
        public string Currency { get; init; }
        public bool IsActive { get; init; }
        public Guid AccountId { get; init; }
    }
}
