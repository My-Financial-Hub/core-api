namespace FinancialHub.Core.Domain.DTOS.Accounts
{
    public class UpdateAccountDto
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public UpdateAccountDto(string name, string description, bool isActive)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
        }
    }
}
