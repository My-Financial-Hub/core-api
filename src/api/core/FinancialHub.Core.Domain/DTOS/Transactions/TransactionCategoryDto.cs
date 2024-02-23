namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class TransactionCategoryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public bool IsActive { get; init; }
    }
}
