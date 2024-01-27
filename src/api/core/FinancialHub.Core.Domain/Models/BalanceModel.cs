namespace FinancialHub.Core.Domain.Models
{
    public record class BalanceModel : BaseModel
    {
        public string Name { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public Guid AccountId { get; set; }
        public AccountModel Account { get; set; }
        public bool IsActive { get; set; }

        public BalanceModel()
        {
            
        }

        public BalanceModel(string name, string currency, decimal amount, AccountModel account, bool isActive)
        {
            Name = name;
            Currency = currency;
            Amount = amount;
            AccountId = account.Id.GetValueOrDefault();
            Account = account;
            IsActive = isActive;
        }

        public BalanceModel(string name, string currency, AccountModel account, bool isActive) : 
            this(name, currency, 0, account, isActive){ }

        public BalanceModel(string currency, AccountModel account, bool isActive) :
            this($"{account!.Name} Default Balance", currency, 0, account, isActive)
        { }

        public BalanceModel(AccountModel account) :
            this(string.Empty, account, account.IsActive)
        { }

        public static BalanceModel CreateDefaultToAccount(AccountModel account)
        {
            return new BalanceModel(account);
            return new BalanceModel(
                name: $"{account!.Name} Default Balance",
                currency: string.Empty,
                account: account,
                isActive: account.IsActive
            );
        }
    }
}
