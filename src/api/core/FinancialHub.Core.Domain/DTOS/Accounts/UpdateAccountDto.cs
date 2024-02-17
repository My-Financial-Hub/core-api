namespace FinancialHub.Core.Domain.DTOS.Accounts
{
    public class UpdateAccountDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public bool IsActive { get; init; }
    }
}
