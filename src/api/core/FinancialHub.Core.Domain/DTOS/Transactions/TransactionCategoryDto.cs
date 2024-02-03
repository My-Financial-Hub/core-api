namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class TransactionCategoryDto
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public TransactionCategoryDto()
        {
            
        }
    }
}
