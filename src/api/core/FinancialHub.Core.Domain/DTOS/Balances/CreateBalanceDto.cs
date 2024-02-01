namespace FinancialHub.Core.Domain.DTOS.Balances
{
    public class CreateBalanceDto
    {
        public string Name { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; private set; }
        public Guid AccountId { get; private set; }

        public CreateBalanceDto(string name, string currency, bool isActive, Guid accountId)
        {
            Name = name;
            Currency = currency;
            IsActive = isActive;
            AccountId = accountId;
        }
    }
}
