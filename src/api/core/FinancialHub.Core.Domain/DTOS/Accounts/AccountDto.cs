namespace FinancialHub.Core.Domain.DTOS.Accounts
{
    public record class AccountDto(
        Guid Id, 
        string Name, 
        string Description, 
        bool IsActive, 
        List<AccountBalanceDto> Balances
    );
}
