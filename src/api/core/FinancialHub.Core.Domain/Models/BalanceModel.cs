using System.Linq;

namespace FinancialHub.Core.Domain.Models
{
    public class BalanceModel : BaseModel
    {
        public string Name { get; private set; }
        public string Currency { get; private set; }
        public decimal Amount { get; private set; }
        public Guid AccountId { get; private set; }
        [Obsolete]
        public AccountModel? Account { get; private set; }
        public bool IsActive { get; private set; }

        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' não pode ser nulo nem vazio.", nameof(name));
            }
        }

        public BalanceModel(Guid? id, string name, decimal amount, string currency, Guid accountId, bool isActive) : base(id)
        {
            Validate(name);
            Name = name;
            Amount = amount;
            AccountId = accountId;
            Currency = currency;
            IsActive = isActive;
        }

        public BalanceModel(Guid? id, string name, string currency, Guid accountId, bool isActive) : 
            this(id, name, 0, currency, accountId, isActive)
        { }

        public void AddAmount(decimal amount)
        {
            if(amount < 0)
            {
                throw new ArgumentException("Amount cannot be smaller than 0", nameof(amount));
            }

            this.Amount += amount;
        }

        public void RemoveAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount cannot be smaller than 0", nameof(amount));
            }

            this.Amount -= amount;
        }

        public static BalanceModel CreateDefault(Guid accountId,string accountName, bool isActive)
        {
            return new BalanceModel(
                null,
                $"{accountName} Default Balance",
                string.Empty,
                accountId,
                isActive
            );
        }
    }
}
