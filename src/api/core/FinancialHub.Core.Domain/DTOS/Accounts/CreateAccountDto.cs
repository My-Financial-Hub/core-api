namespace FinancialHub.Core.Domain.DTOS.Accounts
{
    public class CreateAccountDto
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public CreateAccountDto()
        {
            
        }

        public CreateAccountDto(string name, string description, bool isActive)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
        }
    }
}
