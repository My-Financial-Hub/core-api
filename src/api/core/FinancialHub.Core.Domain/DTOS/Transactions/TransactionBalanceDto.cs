namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class TransactionBalanceDto
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Currency { get; private set; }
        public TransactionAccountDto Account { get; private set; }
        public TransactionBalanceDto()
        {
            
        }
    }
}
