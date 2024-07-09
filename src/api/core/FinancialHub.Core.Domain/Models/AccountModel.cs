namespace FinancialHub.Core.Domain.Models
{
    public class AccountModel : BaseModel
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
        }

        private AccountModel() { }

        public AccountModel(Guid? id, string name, string description, bool isActive) : base(id)
        {
            Validate(name);
            Name = name;
            Description = description;
            IsActive = isActive;
        }

        public AccountModel(string name, string description, bool isActive)
            : this(null, name, description, isActive) { }
    }
}
