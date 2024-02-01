namespace FinancialHub.Core.Domain.DTOS.Balances
{
    public class UpdateBalanceDto
    {
        public string Name { get; set; }
        public string Currency { get; set; }
        public Guid AccountId { get; private set; }
        public bool IsActive { get; private set; }

        public UpdateBalanceDto(string name, string currency, Guid accountId, bool isActive)
        {
            Name = name;
            Currency = currency;
            AccountId = accountId;
            IsActive = isActive;
        }
    }
}
