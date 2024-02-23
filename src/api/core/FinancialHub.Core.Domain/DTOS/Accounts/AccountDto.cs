namespace FinancialHub.Core.Domain.DTOS.Accounts
{
    public class AccountDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public bool IsActive { get; init; }
        public List<AccountBalanceDto> Balances { get; init; }
    }
}
