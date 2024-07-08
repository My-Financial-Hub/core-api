namespace FinancialHub.Core.Domain.Models
{
    public class AccountModel : BaseModel
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        //public List<BalanceModel> Balances { get; private set; }

        //public decimal Total => this.Balances.Sum(a => a.Amount);

        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
        }

        private AccountModel() { }

        public AccountModel(Guid? id, string name, string description, bool isActive, List<BalanceModel>? balances) : base(id)
        {
            Validate(name);
            Name = name;
            Description = description;
            IsActive = isActive;
            //Balances = balances ?? new List<BalanceModel>();
        }

        public AccountModel(string name, string description, bool isActive, List<BalanceModel>? balances)
            : this(null, name, description, isActive, balances) { }

        public void AddBalance(BalanceModel balance)
        {
            //Balances.Add(balance);
        }
    }
}
