namespace FinancialHub.Core.Domain.DTOS.Balances
{
    public class BalanceDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Currency { get; init; }
        public decimal Amount { get; init; }
        public bool IsActive { get; init; }
        public BalanceAccountDto Account { get; init; }
    }
}
