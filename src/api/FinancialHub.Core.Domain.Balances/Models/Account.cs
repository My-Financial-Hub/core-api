namespace FinancialHub.Core.Domain.Accounts.Models
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        private ICollection<Balance> Balances { get; set; }

        private Balance GetBalanceById(Guid balanceId)
        {
            return this.Balances.FirstOrDefault(x => x.Id == balanceId) ?? throw new IndexOutOfRangeException($"Balance with id {balanceId} not found");
        }

        public void AddBalance(string name, string currency, decimal amount)
        {
            this.Balances.Add(new Balance(Guid.NewGuid(), name, currency, amount, this.IsActive));
        }

        public void RemoveBalance(Guid balanceId)
        {
            this.Balances = this.Balances.Where(x => x.Id != balanceId).ToArray();
        }

        public void IncreaseAmount(Guid balanceId, decimal amount)
        {
            var balance = this.GetBalanceById(balanceId);
            balance.IncreaseAmount(amount);
        }

        public void DecreaseAmount(Guid balanceId, decimal amount)
        {
            var balance = this.GetBalanceById(balanceId);
            balance.DecreaseAmount(amount);
        }
    }
}
