using FinancialHub.Core.Domain.Enums;
using System.Linq;

namespace FinancialHub.Core.Domain.Models
{
    public class BalanceModel : BaseModel
    {
        public string Name { get; private set; }
        public string Currency { get; private set; }
        public decimal Amount { get; private set; }
        [Obsolete]
        public Guid AccountId { get; private set; }
        public AccountModel Account { get; private set; }
        public bool IsActive { get; private set; }
        public List<TransactionModel> Transactions { get; private set; }

        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
        }

        public BalanceModel(Guid? id, string name, decimal amount, string currency, AccountModel account, List<TransactionModel> transactions, bool isActive) : base(id)
        {
            Validate(name);
            Name = name;
            Amount = amount;
            Currency = currency;
            Account = account;
            IsActive = isActive;
            Transactions = transactions ?? new List<TransactionModel>();
        }

        public BalanceModel(Guid? id, string name, string currency, AccountModel account, List<TransactionModel> transactions, bool isActive) :
            this(id, name, 0, currency, account, transactions, isActive)
        { }

        public static BalanceModel CreateDefault(AccountModel account)
        {
            return new BalanceModel(
                null,
                $"{account.Name} Default Balance",
                string.Empty,
                account,
                new List<TransactionModel>(),
                account.IsActive
            );
        }

        public void AddTransaction(TransactionModel transaction)
        {
            if (this.Transactions.Any(t => t.Id == transaction.Id))
            {
                throw new Exception("Transaction already exists");
            }

            if (transaction.IsPaid)
            {
                this.Amount = transaction.Type == TransactionType.Earn ?
                    this.Amount + transaction.Amount :
                    this.Amount - transaction.Amount;
            }

            this.Transactions.Add(transaction);
        }

        public void UpdateTransaction(TransactionModel transaction)
        {
            if (transaction.Id == null)
            {
                throw new NullReferenceException(nameof(transaction.Id));
            }

            var existingTransaction = this.Transactions.FirstOrDefault(x => x.Id == transaction.Id) ??
                throw new Exception("No existing transaction to update");

            this.RemoveTransaction(existingTransaction);
            this.AddTransaction(transaction);
        }

        public void RemoveTransaction(TransactionModel transaction)
        {
            if (!this.Transactions.Any(t => t.Id == transaction.Id))
            {
                throw new InvalidOperationException();
            }

            if (transaction.IsPaid)
            {
                this.Amount = transaction.Type == TransactionType.Earn ?
                    this.Amount - transaction.Amount :
                    this.Amount + transaction.Amount;
            }

            this.Transactions.RemoveAll(t => t.Id == transaction.Id);
        }
    }
}
