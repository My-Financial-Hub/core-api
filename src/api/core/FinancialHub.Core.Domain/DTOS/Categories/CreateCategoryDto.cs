namespace FinancialHub.Core.Domain.DTOS.Categories
{
    public class CreateCategoryDto
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public CreateCategoryDto(string name, string description, bool isActive)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
        }
    }
}
