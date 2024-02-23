namespace FinancialHub.Core.Domain.DTOS.Balances
{
    public class BalanceAccountDto
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public BalanceAccountDto()
        {

        }
    }
}
