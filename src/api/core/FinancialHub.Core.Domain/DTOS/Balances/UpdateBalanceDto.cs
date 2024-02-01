namespace FinancialHub.Core.Domain.DTOS.Balances
{
    public class UpdateBalanceDto
    {
        public string Name { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; private set; }

        public UpdateBalanceDto(string name, string currency, bool isActive)
        {
            Name = name;
            Currency = currency;
            IsActive = isActive;
        }
    }
}
