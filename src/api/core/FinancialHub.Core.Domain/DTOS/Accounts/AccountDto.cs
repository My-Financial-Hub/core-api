namespace FinancialHub.Core.Domain.DTOS.Accounts
{
    public class AccountDto
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public List<AccountBalanceDto> Balances { get; private set; }
        public AccountDto()
        {
            
        }
    }
}
