namespace FinancialHub.Core.Domain.DTOS.Balances
{
    public class BalanceDto
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; private set; }
        public BalanceAccountDto Account { get; private set; }
    }
}
