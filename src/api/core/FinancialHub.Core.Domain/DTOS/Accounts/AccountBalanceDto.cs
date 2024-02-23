namespace FinancialHub.Core.Domain.DTOS.Accounts
{
    public class AccountBalanceDto
    {
        public string Name { get; init; }
        public string Currency { get; init; }
        public decimal Amount { get; init; }
        public bool IsActive { get; init; }
    }
}
