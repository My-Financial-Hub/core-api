namespace FinancialHub.Core.Domain.DTOS.Transactions
{
    public class TransactionBalanceDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Currency { get; init; }
        public TransactionAccountDto Account { get; init; }
    }
}
