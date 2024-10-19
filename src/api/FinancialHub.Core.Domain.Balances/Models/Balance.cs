namespace FinancialHub.Core.Domain.Accounts.Models
{
    internal class Balance
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Currency { get; private set; }
        public decimal Amount { get; private set; }
        public bool IsActive { get; private set; }

        public Balance(Guid id, string name, string currency, decimal amount, bool isActive)
        {
            this.Id = id;
            this.Name = name;
            this.Currency = currency;
            this.Amount = amount;
            this.IsActive = isActive;
        }

        public void IncreaseAmount(decimal amount) 
        {
            this.Amount += amount;
        }

        public void DecreaseAmount(decimal amount) 
        { 
            if(this.Amount - amount < 0)
            {
                throw new ArgumentException("Amount cannot be smaller than zero",nameof(amount));
            }
            this.Amount -= amount;
        }
    }
}
