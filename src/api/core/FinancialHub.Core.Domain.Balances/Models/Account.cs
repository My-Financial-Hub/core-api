using FinancialHub.Common.Models;

namespace FinancialHub.Core.Domain.Balances.Models
{
    public class Account : BaseModel
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public List<Balance> Balances { get; private set; }

        public decimal Total => this.Balances.Sum(a => a.Amount);

        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
        }

        public Account() { }

        public Account(Guid? id, string name, string description, bool isActive, List<Balance> balances) : base(id)
        {
            Validate(name);
            Name = name;
            Description = description;
            IsActive = isActive;
            Balances = balances ?? new List<Balance>();
        }

        public Account(string name, string description, bool isActive, List<Balance> balances) 
            : this(null, name, description, isActive, balances) { }

        public void AddBalance(Balance balance)
        {
            Balances.Add(balance);
        }

        public Balance? this[Guid id]
        {
            get
            {
                return this.Balances.FirstOrDefault(x => x.Id == id);
            }
        }
    }
}
