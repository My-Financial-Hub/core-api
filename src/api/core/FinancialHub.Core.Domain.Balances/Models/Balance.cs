using FinancialHub.Common.Models;
using FinancialHub.Core.Domain.Balances.Enums;

namespace FinancialHub.Core.Domain.Balances.Models
{
    public class Balance : BaseModel
    {
        public string Name { get; private set; }
        public string Currency { get; private set; }
        public decimal Amount { get; private set; }
        public Account Account { get; private set; }
        public bool IsActive { get; private set; }
        public List<Transaction> Transactions { get; private set; }

        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
        }

        public Balance(Guid? id, string name, decimal amount, string currency, List<Transaction> transactions, bool isActive) : base(id)
        {
            Validate(name);
            Name = name;
            Amount = amount;
            Currency = currency;
            IsActive = isActive;
            Transactions = transactions ?? new List<Transaction>();
        }

        public Balance(Guid? id, string name, string currency, List<Transaction> transactions, bool isActive) : 
            this(id, name, 0, currency, transactions, isActive)
        { }

        public static Balance CreateDefault(Account account)
        {
            return new Balance(
                null,
                $"{account.Name} Default Balance",
                string.Empty,
                new List<Transaction>(),
                account.IsActive
            );
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction.IsPaid)
            {
                if (transaction.Type == TransactionType.Earn)
                    this.Amount += transaction.Amount;
                else
                    this.Amount -= transaction.Amount;
            }
            else
            {
                if (transaction.Type == TransactionType.Earn)
                    this.Amount -= transaction.Amount;
                else
                    this.Amount += transaction.Amount;
            }
        }

        public void UpdateTransaction(Transaction transaction)
        {
            if (transaction.Id == null)
            {
                throw new NullReferenceException(nameof(transaction.Id));
            }

            var existingTransaction = this.Transactions.FirstOrDefault() ?? 
                throw new Exception("No existing transaction to update");
            
            this.Transactions.Remove(existingTransaction);

            //TODO: add add remove logic
        }
    }
}
