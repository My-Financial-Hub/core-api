namespace FinancialHub.Core.Domain.DTOS.Accounts
{
    public class AccountBalanceDto
    {
        public string Name { get; private set; }
        public string Currency { get; private set; }
        public decimal Amount { get; private set; }
        public bool IsActive { get; private set; }

        public AccountBalanceDto(string name, string currency, decimal amount, bool isActive)
        {
            Name = name;
            Currency = currency;
            Amount = amount;
            IsActive = isActive;
        }
    }
}
