namespace FinancialHub.Core.Domain.DTOS.Accounts
{
    public record class AccountBalanceDto(
        string Name,
        string Currency,
        decimal Amount,
        bool IsActive
    );
}
