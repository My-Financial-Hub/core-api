namespace FinancialHub.Core.Domain.Models
{
    public record class AccountModel : BaseModel 
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public ICollection<BalanceModel> Balances { get; private set; }

        public AccountModel()
        {
            this.Balances = new List<BalanceModel>();
        }

        public AccountModel(string name, string description, bool isActive, ICollection<BalanceModel> balances)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
            Balances = balances;
        }

        public void AddBalance(BalanceModel balance)
        {
            //todo: maybe a check logic?
            this.Balances.Add(balance);
        }
    }
}
